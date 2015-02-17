using System;

namespace SRF.Service
{

	[AttributeUsage(AttributeTargets.Class)]
	public sealed class ServiceAttribute : Attribute
	{

		public Type ServiceType { get; private set; }

		public ServiceAttribute(Type serviceType)
		{
			ServiceType = serviceType;
		}

	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ServiceSelectorAttribute : Attribute
	{

		public Type ServiceType { get; private set; }

		public ServiceSelectorAttribute(Type serviceType)
		{
			ServiceType = serviceType;
		}

	}

	[AttributeUsage(AttributeTargets.Method)]
	public sealed class ServiceConstructorAttribute : Attribute
	{

		public Type ServiceType { get; private set; }

		public ServiceConstructorAttribute(Type serviceType)
		{
			ServiceType = serviceType;
		}

	}

}