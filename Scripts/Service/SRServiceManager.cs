using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SRF.Components;
using UnityEngine;

namespace SRF.Service
{

	[AddComponentMenu(Internal.ComponentMenuPaths.SRServiceManager)]
	public class SRServiceManager : SRAutoSingleton<SRServiceManager>
	{

		/// <summary>
		/// Is there a service loading?
		/// </summary>
		public static bool IsLoading { get { return LoadingCount > 0; } }

		public static int LoadingCount = 0;

		public static T GetService<T>() where T : class
		{

			var s = GetServiceInternal(typeof(T)) as T;

			if(s == null)
				Debug.LogWarning("Service {0} not found. (HasQuit: {1})".Fmt(typeof(T).Name, _hasQuit));

			return s;

		}

		public static object GetService(Type t)
		{

			var s = GetServiceInternal(t);

			if(s == null)
				Debug.LogWarning("Service {0} not found. (HasQuit: {1})".Fmt(t.Name, _hasQuit));

			return s;

		}

		private static object GetServiceInternal(Type t)
		{

			if (_hasQuit || !Application.isPlaying)
				return null;

			var services = Instance._services;

			for (int i = 0; i < services.Count; i++) {

				var s = services[i];

				if (t.IsAssignableFrom(s.Type)) {

					if (s.Object == null) {

						UnRegisterService(t);
						break;

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

			if (_hasQuit || !Application.isPlaying)
				return false;

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

			if (_hasQuit)
				return;

			if (HasService(t)) {

				if (GetServiceInternal(t) == service)
					return;

				throw new Exception("Service already registered for type " + t.Name);

			}

			UnRegisterService(t);

			if (!t.IsInstanceOfType(service)) {
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

			if (_hasQuit || !HasInstance)
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

			public Func<Type> Selector;
			public Func<object> Constructor;

			public override string ToString()
			{

				var s = InterfaceType.Name + " (";

				if (Type != null)
					s += "Type: " + Type.Name;
				else if (Selector != null)
					s += "Selector: " + Selector.Method;
				else if (Constructor != null)
					s += "Constructor: " + Constructor.Method;

				s += ")";

				return s;

			}

		}

		private readonly SRList<Service> _services = new SRList<Service>();

		private List<ServiceStub> _serviceStubs;

		private static bool _hasQuit;

		protected override void Awake()
		{
			_hasQuit = false;
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

				if (attrib != null) {

					_serviceStubs.Add(new ServiceStub {
						Type = type,
						InterfaceType = attrib.ServiceType
					});

				}

				ScanTypeForConstructors(type, _serviceStubs);
				ScanTypeForSelectors(type, _serviceStubs);

			}

			var serviceStrings =
				_serviceStubs.Select(p => "	{0}".Fmt(p)).ToArray();

			Debug.Log("[SRServiceManager] Services Discovered: {0} \n  {1}".Fmt(serviceStrings.Length,
				string.Join("\n  ", serviceStrings)));

		}

		protected object AutoCreateService(Type t)
		{

			UpdateStubs();

			foreach (var stub in _serviceStubs) {

				if (stub.InterfaceType != t)
					continue;

				object service = null;

				if (stub.Constructor != null) {

					service = stub.Constructor();

				} else {

					Type serviceType = stub.Type;

					if (serviceType == null) {
						serviceType = stub.Selector();
					}

					service = DefaultServiceConstructor(t, serviceType);

				}

				if(!HasService(t))
					RegisterService(t, service);

				Debug.Log("[SRServiceManager] Auto-created service: {0} ({1})".Fmt(stub.Type, stub.InterfaceType), service as UnityEngine.Object);

				return service;
			}

			return null;

		}

		protected void OnApplicationQuit()
		{
			_hasQuit = true;
		}

		private static object DefaultServiceConstructor(Type serviceIntType, Type implType)
		{

			// If mono-behaviour based, create a gameobject for this service
			if (typeof (MonoBehaviour).IsAssignableFrom(implType)) {

				var go = new GameObject("_S_" + serviceIntType.Name);
				return go.AddComponent(implType);

			}

			// Otherwise just create an instance
			return Activator.CreateInstance(implType);

		}

		private static void ScanTypeForSelectors(Type t, List<ServiceStub> stubs)
		{

			var methods = t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);

			foreach (var method in methods) {

				var attrib = Attribute.GetCustomAttribute(method, typeof(ServiceSelectorAttribute)) as ServiceSelectorAttribute;

				if (attrib == null)
					continue;

				if (method.ReturnType != typeof (Type)) {
					Debug.LogError("ServiceSelector must have return type of Type ({0}.{1}())".Fmt(t.Name, method.Name));
					continue;
				}

				if (method.GetParameters().Length > 0) {
					Debug.LogError("ServiceSelector must have no parameters ({0}.{1}())".Fmt(t.Name, method.Name));
					continue;
				}

				var stub = stubs.FirstOrDefault(p => p.InterfaceType == attrib.ServiceType);

				if (stub == null) {

					stub = new ServiceStub() {
						InterfaceType = attrib.ServiceType
					};

					stubs.Add(stub);

				}

				stub.Selector = (Func<Type>) Delegate.CreateDelegate(typeof(Func<Type>), method);

			}

		}

		private static void ScanTypeForConstructors(Type t, List<ServiceStub> stubs)
		{

			var methods = t.GetMethods(BindingFlags.Static);

			foreach (var method in methods) {

				var attrib = Attribute.GetCustomAttribute(method, typeof(ServiceConstructorAttribute)) as ServiceConstructorAttribute;

				if (attrib == null)
					continue;

				if (method.ReturnType != attrib.ServiceType) {
					Debug.LogError("ServiceConstructor must have return type of {2} ({0}.{1}())".Fmt(t.Name, method.Name, attrib.ServiceType));
					continue;
				}

				if (method.GetParameters().Length > 0) {
					Debug.LogError("ServiceConstructor must have no parameters ({0}.{1}())".Fmt(t.Name, method.Name));
					continue;
				}

				var stub = stubs.FirstOrDefault(p => p.InterfaceType == attrib.ServiceType);

				if (stub == null) {

					stub = new ServiceStub() {
						InterfaceType = attrib.ServiceType
					};

					stubs.Add(stub);

				}

				stub.Constructor = (Func<object>)Delegate.CreateDelegate(t, method);

			}

		}

	}

}