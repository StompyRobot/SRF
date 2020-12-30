using System;

namespace SRF.Helpers
{
    using System.Reflection;

    public sealed class MethodReference
    {
        public string MethodName { get; private set; }

        private readonly Func<object[], object> _method;

        public MethodReference(object target, MethodInfo method)
        {
            SRDebugUtil.AssertNotNull(target);

            MethodName = method.Name;
            _method = o => method.Invoke(target, o);
        }

        public MethodReference(string methodName, Func<object[], object> method)
        {
            MethodName = methodName;
            _method = method;
        }

        public object Invoke(object[] parameters)
        {
            return _method.Invoke(parameters);
        }
    }
}
