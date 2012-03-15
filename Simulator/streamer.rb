require 'net/http'
require './long_pull.rb'
require 'socket'
require 'rexml/document'
require 'time'

module MTConnect
  class Streamer
    def initialize(url)
      @url = url
    end
    
    def parse(xml, &block)
      nxt, instance = nil
      document = REXML::Document.new(xml)
      document.each_element('//Header') { |x| 
        nxt = x.attributes['nextSequence'].to_i 
        instance = x.attributes['instanceId'].to_i
      }
      
      document.each_element('//Events/*|//Samples/*') do |e|
        block.call(e.name, e.text.to_s)
      end
      document.each_element('//Condition/*') do |e|
        block.call(e.name, "#{e.attributes['type']}_#{e.attributes['nativeCode']}")
      end
      [nxt, instance] 
    end

    def current(client, path, &block)
      current = path.dup + 'current'
      resp = client.get(current)
      parse(resp.body, &block)
    end
    
    def stop
      @running = false
    end

    def start(&block)
      @running = true
      Thread.new do
        begin
          dest = URI.parse(@url)
          client = Net::HTTP.new(dest.host, dest.port)

          path = dest.path
          path += '/' unless path[-1] == ?/
          rootPath = path.dup

          nxt, instance = current(client, rootPath, &block)
          puts "Instance Id: #{instance}"

          puts "polling..."
          begin
            path = rootPath + "sample?interval=0&count=1000&from=#{nxt}&heartbeat=1000"
            puller = LongPull.new(client)
            puller.long_pull(path) do |xml|
              nxt = parse(xml, &block)
              break unless @running
            end
          
            if @running          
              puts "Reconnecting"
              client = Net::HTTP.new(dest.host, dest.port)
              newNxt, newInstance = current(client, rootPath)
              if instance != newInstance
                nxt = newNxt
                instance = newInstance
              end
            end
          rescue 
            puts "Error occurred: #{$!}\n retrying..."
            sleep 1
          end while @running
        rescue 
          puts "Error occurred: #{$!}\n retrying..."
          sleep 1
        end while @running
      end
    end
  end
end