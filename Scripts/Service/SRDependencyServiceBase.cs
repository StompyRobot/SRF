//#define ENABLE_LOGGING
using System;
using System.Collections;
using UnityEngine;

namespace Scripts.Framework.Service
{

	/// <summary>
	/// A service which has async-loading dependencies
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SRDependencyServiceBase<T> : SRServiceBase<T>, IAsyncService where T : class
	{

#if ENABLE_LOGGING
		private const bool Logging = true;
#else
		private const bool Logging = false;
#endif

		public bool IsLoaded
		{
			get { return _isLoaded; }
		}

		protected abstract Type[] Dependencies { get; }
		private bool _isLoaded;

		protected override void Start()
		{

			base.Start();

			StartCoroutine(LoadDependencies());

		}

		/// <summary>
		/// Invoked once all dependencies are loaded
		/// </summary>
		protected virtual void OnLoaded()
		{
			
		}

		private IEnumerator LoadDependencies()
		{

			SRServiceManager.LoadingCount++;

			if (Logging)
				Debug.Log("[Service] Loading service ({0})".Fmt(GetType().Name), this);

			foreach (var d in Dependencies) {

				var hasService = SRServiceManager.HasService(d);

				if (Logging)
					Debug.Log("[Service] Resolving Service ({0}) HasService: {1}".Fmt(d.Name, hasService), this);

				if(hasService)
					continue;

				var service = SRServiceManager.GetService(d);

				if (service == null) {
					Debug.LogError("[Service] Could not resolve dependency ({0})".Fmt(d.Name));
					enabled = false;
					yield break;
				}

				var a = service as IAsyncService;

				if (a != null) {

					while (!a.IsLoaded)
						yield return new WaitForEndOfFrame();

				}

			}

			if (Logging)
				Debug.Log("[Service] Loading service ({0}) complete.".Fmt(GetType().Name), this);

			_isLoaded = true;
			SRServiceManager.LoadingCount--;

			OnLoaded();

		}


	}

}