using System;
using System.Dynamic;

namespace Vienna.SharpScript
{
    [Serializable]
    public class DynamicScriptProxy : DynamicObject
    {
        private readonly ScriptProxy _proxy;

        public DynamicScriptProxy(ScriptProxy proxy)
        {
            _proxy = proxy;
        }

        public string ProxyContext
        {
            get { return _proxy.ProxyContext; }
        }

        public object InvokeMethod(string methodName, object[] args)
        {
            return _proxy.InvokeMethod(methodName, args);
        }

        public object GetProperty(string propertyName)
        {
            return _proxy.GetProperty(propertyName);
        }

        public object SetProperty(string propertyName, object value)
        {
            return _proxy.SetProperty(propertyName, value);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = _proxy.GetProperty(binder.Name);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            try
            {
                _proxy.SetProperty(binder.Name, value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            try
            {
                result = _proxy.InvokeMethod(binder.Name, args);
                return true;
            }
            catch (Exception)
            {
                result = null;
                return false;
            }
        }
    }
}