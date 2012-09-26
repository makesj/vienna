using System;
using System.Reflection;

namespace Vienna.SharpScript
{
    public class ScriptProxy : MarshalByRefObject
    {
        private readonly Type _type;
        private readonly Object _instance;

        public string ProxyContext
        {
            get { return AppDomain.CurrentDomain.FriendlyName; }
        }

        public ScriptProxy(object instance)
        {
            _type = instance.GetType();
            _instance = instance;
        }

        public object InvokeMethod(string methodName, params object[] args)
        {
            try
            {
                return _type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, _instance, args);
            }
            catch (TargetInvocationException ex)
            {
                LogError(ex, methodName);
                throw;
            }
        }

        public object GetProperty(string propertyName)
        {
            try
            {
                return _type.InvokeMember(propertyName, BindingFlags.GetProperty, null, _instance, null);   
            }
            catch (TargetInvocationException ex)
            {
                LogError(ex, propertyName);
                throw;
            }
        }

        public object SetProperty(string propertyName, object value)
        {
            try
            {
                return _type.InvokeMember(propertyName, BindingFlags.SetProperty, null, _instance, new[] { value });
            }
            catch (TargetInvocationException ex)
            {
                LogError(ex, propertyName);
                throw;
            }
        }

        private void LogError(Exception ex, string target)
        {
            Logger.Error("{0}.{1} - Exception! ", _type.Name, target, ex.Message);

            var inner = ex;
            while (inner != null)
            {
                Logger.Log(inner.Message);

                if (inner.InnerException == null)
                {
                    Logger.Log(inner.StackTrace);
                }
                inner = inner.InnerException;
            }
        }

        public DynamicScriptProxy AsDynamic()
        {
            return new DynamicScriptProxy(this);
        }
    }
}