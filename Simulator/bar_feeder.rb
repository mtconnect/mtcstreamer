$: << '.'

require 'rubygems'
require 'adapter'
require 'streamer'
require 'statemachine'
require 'statemachine/generate/dot_graph'

class BarFeederContext
  attr_accessor :statemachine, :link_state, :load_material, :change_material, :chuck_state,
    :fault
  
  include MTConnect
  
  def initialize
    @faults = {}
    @remaining_material = 10
    @remaining_length = 100.0
    @part_length = 7.2
    
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
  
  def activate
    if @link_state == "ENABLED" and @faults.empty? and @chuck_state == "OPEN" 
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
      @statemachine.send(:fault)
      
    when 'Warning', 'Normal'
      @faults.delete(value)
      @statemachine.send(:normal) if @faults.empty?
      
    else
      element = name.split(/([A-Z][a-z]+)/).delete_if(&:empty?).map(&:downcase).join('_')
      mth = "#{element}=".to_sym
      self.send(mth, value) if self.respond_to? mth
      
      action = "#{element}_#{value.downcase}"
      @statemachine.send(action)
    end
  end
  
  def begin_loading
    @adapter.gather do 
      @spindle_interlock_di.value = 'LATCHED'
      @load_material_di.value = 'ACTIVE'
    end
    Thread.new do
      sleep 1
      @statemachine.load_material_completed
    end
  end
  
  def begin_changing
    @adapter.gather do 
      @change_material_di.value = 'ACTIVE'
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
    end
  end

  def interfaces_ready
    @adapter.gather do 
      @load_material_di.value = 'READY'
      @change_material_di.value = 'READY'
    end
  end
  
  def load_completed
    @remaining_length -= @part_length
    puts "*** Remaining length: #{@remaining_length}"
    @adapter.gather do 
      @spindle_interlock_di.value = 'UNLATCHED'
      @load_material_di.value = 'COMPLETE'
      if @remaining_length < @part_length
          @end_of_bar_di.value = 'ACTIVE'
      end
    end
  end
  
  def change_completed
    @adapter.gather do 
      @remaining_length = 100.0
      @remaining_material -= 1
      @remain_di.value = @remaining_material
      @change_material_di.value = 'COMPLETE'
    end
  end
    
  def load_material_failed
    @adapter.gather do 
      @load_material_di.value = 'FAIL'
    end
  end
  
  def change_material_failed
    @adapter.gather do 
      @change_material_di.value = 'FAIL'
    end
  end
  
  def faulted
  end
end

bar_feeder = Statemachine.build do
  startstate :not_ready
  
  # Ways to transition out of not ready state...
  state :not_ready do
    on_entry :interfaces_not_ready
    
    default :not_ready
    event :link_state_enabled, :activated
    event :chuck_state_open, :activated
    event :normal, :activated
    event :load_material_active, :fail
    event :change_material_active, :fail
    event :fault, :fault
  end
  
  state :fault do
    default :fault
    event :normal, :activated, :activate
  end
  
  state :empty do
    default :empty
    event :filled, :activated, :activate
  end

  state :activated do
    on_entry :activate
    event :make_operational, :operational
    event :still_not_ready, :not_ready
  end
    
  superstate :operational do
    startstate :ready
    
    state :ready do
      default :ready
      on_entry :interfaces_ready
      
      event :load_material_active, :load_material
      event :change_material_active, :change_material
    end
    
    state :load_material do
      on_entry :begin_loading
      event :load_material_ready, :load_material_fail
      event :load_material_completed, :load_material_complete
    end
    
    state :change_material do
      on_entry :begin_changing
      event :change_material_ready, :change_material_fail
      event :change_material_completed, :change_material_complete
    end
    
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
    
    # These will auto transition to complete unless they fail.
    state :load_material_complete do
      on_entry :load_completed
      default :load_material_complete
      event :load_material_ready, :ready
    end
    state :change_material_complete do
      on_entry :change_completed
      default :load_material_complete
      event :change_material_ready, :ready
    end
    
    # handle some failure conditions
    event :fail, :fail, :failed
    event :fault, :fault, :faulted
    event :link_state_unavailable, :not_ready
  end
  
  context BarFeederContext.new
end
bar_feeder.tracer = STDOUT
context = bar_feeder.context
context.statemachine = bar_feeder

streamer = MTConnect::Streamer.new('http://localhost:5000/cnc')
thread = streamer.start do |name, value|
  begin
    context.event(name, value)
  rescue
    puts "Error occurred in handling event: #{$!}"
    puts $!.backtrace.join("\n")
  end
end

thread.join