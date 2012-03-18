
require 'rubygems'
require 'adapter'
require 'streamer'
require 'statemachine'
require 'readline'
require 'statemachine/generate/dot_graph'

module BarFeeder
  class BarFeederContext
    attr_accessor :statemachine, :link_state, :load_material, :change_material, :chuck_state,
      :fault
  
    include MTConnect
  
    def initialize
      @faults = {}
      @remaining_material = 5
      @remaining_length = 100.0
      @part_length = 24.2
      @chuck_open = false
    
      @adapter = Adapter.new
      @adapter.data_items << (@availability_di = DataItem.new('avail'))
      @adapter.data_items << (@mode_di = DataItem.new('mode'))
      @adapter.data_items << (@exec_di = DataItem.new('exec'))
      @adapter.data_items << (@load_material_di = DataItem.new('load'))
      @adapter.data_items << (@change_material_di = DataItem.new('change'))
      @adapter.data_items << (@spindle_interlock_di = DataItem.new('s_inter'))
      @adapter.data_items << (@end_of_bar_di = DataItem.new('eob'))
      @adapter.data_items << (@remain_di = DataItem.new('remain'))
      @adapter.data_items << (@system_di = Condition.new('system'))
      @adapter.data_items << (@fill_di = Condition.new('fill'))
    
      @availability_di.value = "AVAILABLE"
      @exec_di.value = 'READY'
      @mode_di.value = 'AUTOMATIC'
      @end_of_bar_di.value = 'READY'
      @spindle_interlock_di.value = 'UNLATCHED'
      @fill_di.normal
      @system_di.normal
    
      @remain_di.value = @remaining_material
    
      @adapter.start    
    end
  
    def stop
      @adapter.stop
    end
  
    def add_conditions
      if @remaining_material <= 0
        @fill_di.add('fault', 'magazine empty', '1', 'LOW')
      elsif @remaining_material <= 2
        @fill_di.add('warning', 'magazine low', '2', 'LOW')
      end
    end
  
    def activate
      if @link_state == "ENABLED" and @faults.empty? 
        puts "Becomming operational"
        @statemachine.make_operational
      else
        puts "Still not ready"
        @statemachine.still_not_ready
      end
    end
  
    def event(name, value)
      case name 
      when "Fault"
        @faults[value] = true
        @statemachine.fault
      
      when 'Warning', 'Normal'
        @faults.delete(value)
        @statemachine.normal if @faults.empty?
            
      when 'DISCONNECTED'
        @statemachine.disconnected
      
      else
        element = name.split(/([A-Z][a-z]+)/).delete_if(&:empty?).map(&:downcase).join('_')
        mth = "#{element}=".to_sym
        self.send(mth, value) if self.respond_to? mth
      
        # Only send valid events tot the statemachine. 
        action = "#{element}_#{value.downcase}".to_sym
        @statemachine.send(action) if @statemachine.respond_to? action
      end
    end
  
    def begin_loading
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
  
    def begin_changing
      @adapter.gather do 
        @change_material_di.value = 'ACTIVE'
        add_conditions
      end
      Thread.new do
        sleep 2
        @statemachine.change_material_completed
      end
    end
  
    def interfaces_not_ready
      @adapter.gather do 
        @load_material_di.value = 'NOT_READY'
        @change_material_di.value = 'NOT_READY'
        add_conditions
      end
    end

    def interfaces_ready
      @adapter.gather do 
        @load_material_di.value = @chuck_open ? 'READY' : 'NOT_READY'
        @change_material_di.value = 'READY'
        add_conditions
      end
    end
  
    def end_of_bar
      @adapter.gather do 
        @load_material_di.value = 'NOT_READY'
        @change_material_di.value = 'READY'
        @end_of_bar_di.value = 'ACTIVE'
        add_conditions
      end
    end

    def load_completed
      @remaining_length -= @part_length
      puts "*** Remaining length: #{@remaining_length}"
      @adapter.gather do 
        @spindle_interlock_di.value = 'UNLATCHED'
        @load_material_di.value = 'COMPLETE'
        add_conditions
      end
      if @remaining_length < @part_length
          @statemachine.bar_finished
      end
    end
  
    def change_completed
      @adapter.gather do 
        @change_material_di.value = 'COMPLETE'
        @end_of_bar_di.value = 'READY'
        @remaining_material -= 1
        @remain_di.value = @remaining_material
        if @remaining_material > 0
          @remaining_length = 100.0
        end
        add_conditions
      end
    end
  
    def refill
      @remaining_material = 5
      @remaining_length = 0
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
  
    def faulted
    end
  
    def chuck_open
      @adapter.gather do 
        @load_material_di.value = 'READY'
        @chuck_open = true
      end
      @statemachine.activate
    end
  
    def chuck_not_open
      @adapter.gather do 
        @load_material_di.value = 'NOT_READY'
        @chuck_open = false
      end
      @statemachine.activate
    end
  end

  @bar_feeder = Statemachine.build do
    startstate :disabled
  
    superstate :base do
      event :chuck_state_open, :chuck_open
      event :chuck_state_unlatched, :chuck_not_open
      event :chuck_state_closed, :chuck_not_open
  
      superstate :disabled do
        default :not_ready
        default_history :not_ready
        on_entry :interfaces_not_ready
    
        # Ways to transition out of not ready state...
        state :not_ready do
          on_entry :interfaces_not_ready
    
          default :not_ready
          event :link_state_enabled, :activated
          event :normal, :activated
          event :load_material_active, :fail
          event :change_material_active, :fail
          event :fault, :fault
        end
  
        state :fault do
          default :fault
          event :normal, :activated
        end
      
        state :fail do
          default :fail
          on_entry :load_material_failed
          event :load_material_fail, :not_ready
        end
  
        state :empty do
          default :empty
          event :filled, :end_of_bar, :refill
        end
      end
  
      state :activated do
        on_entry :activate
        event :make_operational, :operational_H
        event :still_not_ready, :disabled_H
      end
  
      state :chuck_open do
        on_entry :chuck_open
        event :activate, :activated
      end
  
      state :chuck_not_open do
        on_entry :chuck_not_open
        event :activate, :activated
      end
  
      superstate :operational do
        startstate :ready
        default_history :ready
    
        state :ready do
          default :ready
          on_entry :interfaces_ready
      
          event :load_material_active, :load_material
          event :change_material_active, :change_material
        end
    
        state :load_material do
          on_entry :begin_loading
          event :chuck_state_closed, :load_material_fail
          event :chuck_state_unlatched, :load_material_fail
          event :load_material_ready, :load_material_fail
          event :load_material_completed, :load_material_complete
          event :load_material_fail, :cnc_load_failed          
        end
    
        state :change_material do
          on_entry :begin_changing
          event :change_material_ready, :change_material_fail
          event :change_material_completed, :change_material_complete
          event :change_material_fail, :cnc_change_failed    
        end
    
        # Handle invalid CNC state change in which we will respond with a fail
        # This requires CNC ack the fail with a FAIL. When we get the fail, we
        # transition to a ready
        state :load_material_fail do
          on_entry :load_material_failed
          default :load_material_fail
          event :load_material_fail, :ready
        end
    
        state :change_material_fail do
          on_entry :change_material_failed
          default :change_material_fail
          event :change_material_fail, :ready
        end
    
        # Handle CNC failing current operation. We should
        # only get here when we are operational and active.
        state :cnc_load_failed do 
          on_entry :load_material_failed
          default :cnc_load_failed
          event :load_material_ready, :ready
        end
    
        state :cnc_change_failed do 
          on_entry :change_material_failed
          default :cnc_change_failed
          event :change_material_ready, :ready
        end

        # These will auto transition to complete unless they fail.
        state :load_material_complete do
          on_entry :load_completed
          default :load_material_complete
          event :load_material_ready, :ready
          event :bar_finished, :end_of_bar
        end
    
        state :change_material_complete do
          on_entry :change_completed
          default :change_material_complete
          event :change_material_ready, :ready
        end
    
        # End of bar state, end of bar is active and we need a change bar 
        # event
        state :end_of_bar do
          on_entry :end_of_bar
          default :end_of_bar
          event :load_material_active, :load_material_fail_eob
          event :change_material_active, :change_material
        end
    
        state :load_material_fail_eob do
          on_entry :load_material_failed
          default :load_material_fail_eob
          event :load_material_fail, :end_of_bar
        end
    
        # Super fail events
        event :load_material_fail, :cnc_load_failed
        event :change_material_fail, :cnc_load_failed
    
        # handle some failure conditions
        event :disconnected, :not_ready
        event :fault, :fault, :faulted
        event :link_state_unavailable, :not_ready
        event :link_state_disabled, :not_ready
        event :availability_unavailable, :not_ready    
      end
    end
  
    context BarFeederContext.new
  end
  
  def self.bar_feeder
    @bar_feeder
  end
end