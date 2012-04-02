
require 'adapter'
require 'statemachine'

module MTConnect
  class Context
    attr_reader :statemachine
  
    def initialize(port)
      @adapter = Adapter.new(port)
    
      @faults = {}
      @connected = false
    end
    
    def stop
      @adapter.stop
    end
    
    def statemachine=(statemachine)
      @statemachine = statemachine
      class << @statemachine
        @@event_mutex = Mutex.new
        @@thread = nil

        alias _process_event process_event

        def process_event(event, *args)
          if @@thread != Thread.current
            # puts "**** Waiting for lock ****"
            @@event_mutex.lock
            thread = @@thread = Thread.current
          end
          _process_event(event, *args)
        ensure
          if thread
            # puts "**** Releasing lock ****"
            @@thread = nil
            @@event_mutex.unlock
          end        
        end
      end
    end
  
    def event(name, value)
      puts "Received #{name} #{value}"
      case name 
      when "Fault"
        @faults[value] = true
        @statemachine.fault
        @connected = true
    
      when 'Warning', 'Normal'
        @faults.delete(value)
        @statemachine.normal if @faults.empty?
        @connected = true
          
      when 'DISCONNECTED'
        @statemachine.disconnected
        @connected = false
        @adapter.gather do 
          add_conditions
        end
    
      else
        @connected = true
        element = name.split(/([A-Z][a-z]+)/).delete_if(&:empty?).map(&:downcase).join('_')
        mth = "#{element}=".to_sym
        self.send(mth, value) if self.respond_to? mth
    
        # Only send valid events tot the statemachine. 
        action = "#{element}_#{value.downcase}".to_sym
        @statemachine.send(action) if @statemachine.respond_to? action
      end
    end
  end
end