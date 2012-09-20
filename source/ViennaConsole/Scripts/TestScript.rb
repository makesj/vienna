class TestScript
    attr_accessor :myaccessor
    
    def initialize(arg)
        puts "TestScript class initialize method"
        @myaccessor = arg
    end
    
    def instance_method(date)
        puts "instance_method called from c# with date -> #{date}"
        return 12345
    end
    
    def print_myaccessor()
        puts @myaccessor
    end
    
end

def some_global_scope_method(arg)
    puts "some_global_scope_method called from c# with arg -> #{arg}"
end

def call_foo
    test_case.Foo
end