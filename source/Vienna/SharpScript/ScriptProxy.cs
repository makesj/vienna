using System;
using System.Reflection;

namespace Vienna.SharpScript
{
    public class ScriptProxy : MarshalByRefObject
    {
        public Type Type { get; set; } 
        public Object Instance { get; set; }

        public string ProxyContext
        {
            get { return AppDomain.CurrentDomain.FriendlyName; }
        }

        public ScriptProxy(object instance)
        {
            Type = instance.GetType();
            Instance = instance;
        }

        public object InvokeMethod(string methodName, object[] args)
        {
            try
            {
                return Type.InvokeMember(methodName, BindingFlags.InvokeMethod, null, Instance, args);
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
                return Type.InvokeMember(propertyName, BindingFlags.GetProperty, null, Instance, null);   
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
                return Type.InvokeMember(propertyName, BindingFlags.SetProperty, null, Instance, new[] { value });
            }
            catch (TargetInvocationException ex)
            {
                LogError(ex, propertyName);
                throw;
            }
        }

        private void LogError(Exception ex, string target)
        {
            Logger.Error("{0}.{1} - Exception! ", Type.Name, target, ex.Message);

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
    }
}