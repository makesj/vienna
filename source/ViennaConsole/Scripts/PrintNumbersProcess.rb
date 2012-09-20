class Process
    
    def on_init
    end
    
    def on_update(delta) 
    end
    
    def on_success
    end
    
    def on_fail
    end
    
    def on_abort
    end

end

class PrintNumbersProcess < Process

    def initialize()    
        puts "initialize"       
    end

    def init()
        puts "init override"
    end

end