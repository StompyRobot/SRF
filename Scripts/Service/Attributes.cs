using System;
using UnityEngine;
using System.Collections;

namespace Scripts.Framework.Service
{

	[AttributeUsage(AttributeTargets.Class)]
	public class ServiceAttribute : Attribute
	{

		public Type ServiceType { get; private set; }

		public ServiceAttribute(Type serviceType)
		{
			ServiceType = serviceType;
		}

	}

	[AttributeUsage(AttributeTargets.Method)]
	public class ServiceSelectorAttribute : Attribute
	{

		public Type ServiceType { get; private set; }

		public ServiceSelectorAttribute(Type serviceType)
		{
			ServiceType = serviceType;
		}

	}

	[AttributeUsage(AttributeTargets.Method)]
	public class ServiceConstructorAttribute : Attribute
	{

		public Type ServiceType { get; private set; }

		public ServiceConstructorAttribute(Type serviceType)
		{
			ServiceType = serviceType;
		}

	}

}