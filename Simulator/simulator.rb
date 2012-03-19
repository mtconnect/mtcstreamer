$: << '.'

require 'bar_feeder'


BarFeeder.bar_feeder.tracer = STDOUT
context = BarFeeder.bar_feeder.context

streamer = MTConnect::Streamer.new('http://localhost:5000/cnc')
thread = streamer.start do |name, value|
  begin
    context.event(name, value)
  rescue
    puts "Error occurred in handling event: #{$!}"
    puts $!.backtrace.join("\n")
  end
end

# Command processor
while true
  begin
    line = Readline.readline('> ', true)    
    if line.nil? or line == 'quit'
      context.stop
      streamer.stop
      exit 0
    end
    next if line.empty?
    
    # send the line as an event...
    event = line.strip.to_sym
    if BarFeeder.bar_feeder.respond_to? event
      BarFeeder.bar_feeder.send(event)
    else
      puts "Bar feeder does does not recognize #{event} in state #{bar_feeder.state}"
    end
  rescue
    puts "Error: #{$!}"
  end
end

thread.join