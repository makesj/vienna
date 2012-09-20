#core game classes and methods go here

class RubyProcess
    
    def initialize(interop)
        @interop = interop
    end
    
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
    
    def is_alive()
        return @interop.IsAlive()
    end
    
    def is_dead()
        return @interop.IsDead()
    end
    
    def is_paused()
        return @interop.IsPaused()
    end
    
    def attach_child(process)
        return @interop.AttachChildFromScript(process)
    end

end