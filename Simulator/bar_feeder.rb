$: << '.'

require 'rubygems'
require 'adapter'
require 'streamer'
require 'statemachine'
require 'readline'
require 'statemachine/generate/dot_graph'
require 'mtc_context'

module BarFeeder
  class BarFeederContext < MTConnect::Context
    attr_accessor :link_state, :load_material, :change_material, :chuck_state, :fault
  
    include MTConnect
  
    def initialize(port = 7878)
      super(port)
      
      @adapter.data_items << (@availability_di = DataItem.new('avail'))
      @adapter.data_items << (@mode_di = DataItem.new('mode'))
      @adapter.data_items << (@exec_di = DataItem.new('exec'))
      @adapter.data_items << (@load_material_di = DataItem.new('load'))
      @adapter.data_items << (@change_material_di = DataItem.new('change'))
      @adapter.data_items << (@spindle_interlock_di = DataItem.new('s_inter'))
      @adapter.data_items << (@end_of_bar_di = DataItem.new('eob'))
      @adapter.data_items << (@remain_di = DataItem.new('remain'))
      @adapter.data_items << (@length_di = DataItem.new('length'))
      @adapter.data_items << (@system_di = Condition.new('system'))
      @adapter.data_items << (@fill_di = Condition.new('fill'))
      @adapter.data_items << (@top_cut_di = DataItem.new('top_cut'))

      # Initialize data items
      @availability_di.value = "AVAILABLE"
      @exec_di.value = 'READY'
      @mode_di.value = 'MANUAL'
      @end_of_bar_di.value = 'READY'
      @spindle_interlock_di.value = 'UNLATCHED'
      @remain_di.value = @remaining_material
      @length_di.value = @remaining_length
      @top_cut_di.value = 'READY'
      @fill_di.normal
      @system_di.normal
      
      # Initialize bar feeder state...
      @mag_empty = false
      @remaining_material = 5
      @remaining_length = 0.0
      @part_length = 24.2
      @chuck_open = false
            
      @adapter.start    
    end
  
    def add_conditions
      if @mag_empty
        @fill_di.add('fault', 'magazine empty', '1', 'LOW')
      elsif @remaining_material <= 2
        @fill_di.add('warning', 'magazine low', '2', 'LOW')
      end
      unless @connected
        @system_di.add('fault', 'CNC has disconnected', '3')
      end
    end
  
    def activate
      if @mag_empty
        @statemachine.empty
      elsif @link_state == "ENABLED" and @faults.empty? and @chuck_open
        puts "Becomming operational"
        @statemachine.make_operational
      else
        puts "Still not ready"
        @statemachine.still_not_ready
      end
    end
  
    def interfaces_not_ready
      @adapter.gather do 
        @load_material_di.value = 'NOT_READY'
        @change_material_di.value = 'NOT_READY'
        @top_cut_di.value = 'NOT_READY'
        @mode_di.value = 'MANUAL'
        add_conditions
      end
    end

    def interfaces_ready
      if @remaining_length < @part_length
        @mode_di.value = 'AUTOMATIC'
        @adapter.gather do 
          @load_material_di.value = 'NOT_READY'
        end
        @statemachine.bar_finished
      else          
        @adapter.gather do 
          @load_material_di.value = 'READY'
          @change_material_di.value = 'READY'
          @top_cut_di.value = 'READY'
          @mode_di.value = 'AUTOMATIC'
          add_conditions
        end
      end
    end
    
    def begin_load_material
      unless @chuck_open
        @statemachine.chuck_state_closed
      else
        @adapter.gather do 
          @spindle_interlock_di.value = 'LATCHED'
          @load_material_di.value = 'ACTIVE'
          add_conditions
        end
        Thread.new do
          sleep 1
          @statemachine.load_material_completed
        end
      end
    end
  
    def begin_change_material
      unless @chuck_open
        @statemachine.chuck_state_closed
      else
        @adapter.gather do 
          @change_material_di.value = 'ACTIVE'
          add_conditions
        end
        Thread.new do
          sleep 2
          @statemachine.change_material_completed
        end
      end
    end
      
    def end_of_bar
      if @remaining_material == 0
        puts "**** Bar feeder is empty"
        @mag_empty = true
        @adapter.gather do 
          add_conditions
        end
        @statemachine.empty
      else      
        @adapter.gather do 
          @load_material_di.value = 'NOT_READY'
          @change_material_di.value = @chuck_open ? 'READY' : 'NOT_READY'
          @end_of_bar_di.value = 'ACTIVE'
          add_conditions
        end
      end
    end

    def load_material_completed
      @remaining_length -= @part_length
      puts "**** Remaining length: #{@remaining_length}"
      @adapter.gather do 
        @spindle_interlock_di.value = 'UNLATCHED'
        @load_material_di.value = 'COMPLETE'
        @length_di.value =  @remaining_length
        add_conditions
      end
    end
  
    def change_material_completed
      if @remaining_material > 0
        @remaining_material -= 1
        puts "**** Remaining bars: #{@remaining_material}"
        @remain_di.value = @remaining_material
        @remaining_length = 100.0
      end
      @adapter.gather do 
        @change_material_di.value = 'COMPLETE'
        @end_of_bar_di.value = 'READY'
        @length_di.value =  @remaining_length          
        add_conditions
      end
    end
  
    def refill
      @remaining_material = 5
      @remaining_length = 0
      @mag_empty = false
      @adapter.gather do
        @length_di.value =  @remaining_length          
        @remain_di.value = @remaining_material
        add_conditions
      end
    end
    
    def load_material_failed
      @adapter.gather do 
        @load_material_di.value = 'FAIL'
        add_conditions
      end
    end
  
    def change_material_failed
      @adapter.gather do 
        @change_material_di.value = 'FAIL'
        add_conditions
      end
    end
    
    def reset_history
      @statemachine.get_state(:operational).reset
    end
  
    def faulted
    end
  
    def chuck_open
      @chuck_open = true
    end
  
    def chuck_not_open
      @chuck_open = false
    end
        
    def top_cut
      @adapter.gather do 
        @change_material_di.value = 'READY'
        @load_material_di.value = 'READY'
        @top_cut_di.value = 'ACTIVE'
        add_conditions
      end
    end
    
    def top_cut_completed
      @adapter.gather do 
        @top_cut_di.value = 'READY'
        add_conditions
      end
    end
  end

  @bar_feeder = Statemachine.build do
    startstate :disabled
  
    superstate :base do
      event :chuck_state_open, :activated, :chuck_open
      event :chuck_state_unlatched, :activated, :chuck_not_open
      event :chuck_state_unavailable, :activated, :chuck_not_open
      event :chuck_state_closed, :activated, :chuck_not_open
  
      superstate :disabled do
        default :not_ready
        default_history :not_ready
        on_entry :interfaces_not_ready
    
        # Ways to transition out of not ready state...
        state :not_ready do
          on_entry :interfaces_not_ready
          default :not_ready
          
          event :reset_cnc, :activated
          
          event :link_state_enabled, :activated
          event :normal, :activated
          
          event :load_material_active, :load_fail
          event :change_material_active, :change_fail
          event :fault, :fault
        end
  
        state :fault do
          default :fault
          event :normal, :activated
          event :load_material_active, :load_fail
          event :change_material_active, :change_fail
        end
      
        state :empty do
          default :empty
          event :filled, :activated, :refill
          event :load_material_active, :load_fail
          event :change_material_active, :change_fail
        end
      end
  
      state :activated do
        on_entry :activate
        event :make_operational, :operational_H
        event :still_not_ready, :not_ready
        event :empty, :empty
      end
    
      state :load_fail do
        default :load_fail
        on_entry :load_material_failed
        event :load_material_fail, :disabled_H
      end

      state :change_fail do
        default :change_fail
        on_entry :change_material_failed
        event :change_material_fail, :disabled_H
      end
  
      superstate :operational do
        startstate :ready
        default_history :ready
        
        state :ready do
          default :ready
          on_entry :interfaces_ready
        end
                
        [:load_material, :change_material].each do |interface|
          active = "#{interface}_active".to_sym
          fail = "#{interface}_fail".to_sym
          failed = "#{interface}_failed".to_sym
          complete = "#{interface}_complete".to_sym
          completed = "#{interface}_completed".to_sym
          ready = "#{interface}_ready".to_sym
          cnc_fail = "cnc_#{interface}_fail".to_sym
          
          trans :ready, active, interface
            
          # Active state of interface  
          state interface do
            on_entry "begin_#{interface}".to_sym
            event ready, fail
            event completed, complete
            event fail, cnc_fail
            
            # Handle the chuck state failures
            event :chuck_state_unlatched, fail, :chuck_not_open
            event :chuck_state_unavailable, fail, :chuck_not_open
            event :chuck_state_closed, fail, :chuck_not_open
          end
    
          # Handle invalid CNC state change in which we will respond with a fail
          # This requires CNC ack the fail with a FAIL. When we get the fail, we
          # transition to a ready
          state fail do
            on_entry failed
            default fail
            event fail, :activated, :reset_history
          end
    
          # Handle CNC failing current operation. We should
          # only get here when we are operational and active.
          state cnc_fail do 
            on_entry failed
            default cnc_fail
            event ready, :ready
          end
          
          event fail, cnc_fail
    
          # These will auto transition to complete unless they fail.
          state complete do
            on_entry completed
            default complete
            event ready, :ready
          end
        end
        
        # End of bar state, end of bar is active and we need a change bar 
        # event
        state :end_of_bar do
          on_entry :end_of_bar
          default :end_of_bar
          event :load_material_active, :load_material_fail_eob
          event :change_material_active, :change_material
          event :empty, :empty
        end
  
        state :load_material_fail_eob do
          on_entry :load_material_failed
          default :load_material_fail_eob
          event :load_material_fail, :end_of_bar
        end
        
        # A few other states
        trans :ready, :bar_finished, :end_of_bar
        trans :change_material_complete, :change_material_ready, :top_cut
        
        state :top_cut do
          on_entry :top_cut
          event :top_cut_complete, :ready
          event :top_cut_fail, :top_cut_fail, :top_cut_completed
        end
        
        state :top_cut_fail do
          default :top_cut_fail
          on_entry :top_cut_fail
          event :top_cut_ready, :ready
        end
        
        # handle some failure conditions
        event :disconnected, :disabled
        event :fault, :fault, :faulted
        event :link_state_unavailable, :disabled
        event :link_state_disabled, :disabled
        event :availability_unavailable, :disabled 
      end
    end
    
    context BarFeederContext.new    
  end
    
  def self.bar_feeder
    @bar_feeder
  end
end

if $0 == __FILE__
  module Statemachine
    module Generate
      module DotGraph
        class DotGraphStatemachine
          def explore_sm
            @nodes = []
            @transitions = []
            @sm.states.values.each { |state|
              state.transitions.values.each { |transition|
                @nodes << transition.origin_id
                @nodes << transition.destination_id
                @transitions << transition
              }
            }
            @transitions = @transitions.uniq
            @nodes = @nodes.uniq
          end
        end
      end
    end
  end
  BarFeeder.bar_feeder.to_dot(:output => 'graph')
end