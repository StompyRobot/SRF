using System;
using System.Reflection;

namespace SRF.Helpers
{

	public class MethodReference
	{

		private object _target;
		private MethodInfo _method;

		public string MethodName { get { return _method.Name; } }

		public MethodReference(object target, MethodInfo method)
		{

			SRDebugUtil.AssertNotNull(target);

			_target = target;
			_method = method;

		}

		public object Invoke(object[] parameters)
		{

			return _method.Invoke(_target, parameters);

		}

	}

}