require "CoreLibrary"

class PrintNumbersProcess < RubyProcess

    def initialize(interop)   
        super(interop) 
        puts "initialize"       
    end

    def on_init()
        puts "init override"
    end
    
    def on_update(delta) 
        puts delta
    end

end