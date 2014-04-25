using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Scripts.Framework.Service
{
	
	public class SRServiceManager : SRAutoSingleton<SRServiceManager>
	{

		public static T GetService<T>() where T : class
		{

			var s = GetServiceInternal(typeof(T)) as T;

			if(s == null)
				UnityEngine.Debug.LogWarning("Service " + typeof(T).Name + " not found.");

			return s;

		}

		private static object GetServiceInternal(Type t)
		{

			var services = Instance._services;


			for (int i = 0; i < services.Count; i++) {

				var s = services[i];

				if (t.IsAssignableFrom(s.Type)) {

					if (s.Object == null) {

						UnRegisterService(t);
						return null;

					}

					return s.Object;

				}

			}

			return Instance.AutoCreateService(t);

		}

		public static bool HasService<T>() where T : class
		{

			return HasService(typeof (T));

		}

		public static bool HasService(Type t)
		{

			var services = Instance._services;

			for (int i = 0; i < services.Count; i++) {

				var s = services[i];

				if (t.IsAssignableFrom(s.Type))
					return s.Object != null;

			}

			return false;

		}

		public static void RegisterService<T>(object service) where T : class
		{

			RegisterService(typeof(T), service);

		}

		private static void RegisterService(Type t, object service)
		{

			if (HasService(t)) {

				if (GetServiceInternal(t) == service)
					return;

				throw new Exception("Service already registered for type " + t.Name);

			}

			UnRegisterService(t);

			if (service.GetType().IsAssignableFrom(t)) {
				throw new ArgumentException("service {0} must be assignable from type {1}".Fmt(service.GetType(), t));
			}

			Instance._services.Add(new Service() {
				Object = service,
				Type = t
			});

		}

		public static void UnRegisterService<T>() where T : class
		{
			UnRegisterService(typeof(T));
		}

		private static void UnRegisterService(Type t)
		{

			if (!HasInstance)
				return;

			if (!HasService(t)) {
				return;
			}

			var services = Instance._services;

			for (var i = services.Count - 1; i >= 0; i--) {

				var s = services[i];

				if (s.Type == t)
					services.RemoveAt(i);

			}

		}

		[Serializable]
		private class Service
		{

			public Type Type;
			public object Object;

		}

		[Serializable]
		private class ServiceStub
		{

			public Type InterfaceType;
			public Type Type;

		}

		private SRList<Service> _services = new SRList<Service>();

		private List<ServiceStub> _serviceStubs; 

		protected override void Awake()
		{
			base.Awake();
			DontDestroyOnLoad(CachedGameObject);
		}

		protected void UpdateStubs()
		{

			if (_serviceStubs != null)
				return;

			_serviceStubs = new List<ServiceStub>();

			var types = Assembly.GetExecutingAssembly().GetTypes();

			foreach (var type in types) {

				var attrib = Attribute.GetCustomAttribute(type, typeof (ServiceAttribute)) as ServiceAttribute;

				if(attrib == null)
					continue;

				_serviceStubs.Add(new ServiceStub {
					Type = type,
					InterfaceType = attrib.ServiceType
				});

			}

			var serviceStrings =
				_serviceStubs.Select(p => "	{0} ({1})".Fmt(p.Type, p.InterfaceType.Name)).ToArray();

			Debug.Log("Services Discovered: \n " + string.Join("\n", serviceStrings));

		}

		protected object AutoCreateService(Type t)
		{

			UpdateStubs();

			foreach (var stub in _serviceStubs) {

				if (stub.InterfaceType == t) {

					//var serviceContainer = Hierarchy.Instance["/_Services"];
					//DontDestroyOnLoad(serviceContainer.gameObject);

					var go = new GameObject("_S_" + t.Name);
					//go.transform.parent = serviceContainer;

					var s = (object)go.AddComponent(stub.Type);

					if(!HasService(t))
						RegisterService(t, s);

					Debug.Log("Auto-created service: {0} ({1})".Fmt(stub.Type, stub.InterfaceType), go);

					return s;

				}

			}

			return null;

		}

	}

}