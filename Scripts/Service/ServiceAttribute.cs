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

}