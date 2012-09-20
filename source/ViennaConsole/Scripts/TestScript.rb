class TestScript

    def initialize()
        puts "TestScript class initialize method"
    end
    
    def instance_method(date)
        puts "instance_method called from c# with date -> #{date}"
        return 12345
    end
    
end

def some_global_scope_method(arg)
    puts "some_global_scope_method called from c# with arg -> #{arg}"
end