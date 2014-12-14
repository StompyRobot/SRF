using System;
using System.Reflection;

namespace SRF.Helpers
{

	public class PropertyReference
	{

		private object _target;
		private PropertyInfo _property;

		public string PropertyName { get { return _property.Name; } }
		public Type PropertyType { get { return _property.PropertyType; } } 

		public PropertyReference(object target, PropertyInfo property)
		{

			SRDebug.AssertNotNull(target);

			_target = target;
			_property = property;

		}

		public object GetValue()
		{
			return _property.GetValue(_target, null);
		}

		public void SetValue(object value)
		{
			_property.SetValue(_target, value, null);
		}

	}

}