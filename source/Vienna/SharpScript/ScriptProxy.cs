using System;
using System.Reflection;

namespace Vienna.SharpScript
{
    public class ScriptProxy : MarshalByRefObject
    {
        public Type Type { get; set; } 
        public Object Instance { get; set; }

        public string ProxyDomain
        {
            get { return AppDomain.CurrentDomain.FriendlyName; }
        }

        public ScriptProxy(object wrap)
        {
            Type = wrap.GetType();
            Instance = wrap;
        }

        public object InvokeMethod(string methodName, object[] args)
        {
            return Type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, Instance, args);   
        }

        public object GetProperty(string propertyName)
        {
            return Type.InvokeMember(propertyName, BindingFlags.GetProperty, null, Instance, null);   
        }

        public object SetProperty(string propertyName, object value)
        {
            return Type.InvokeMember(propertyName, BindingFlags.SetProperty, null, Instance, new[]{value});
        }

        public DynamicScriptProxy AsDynamic()
        {
            return new DynamicScriptProxy(this);
        }
    }
}