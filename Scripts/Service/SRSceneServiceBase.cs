//#define ENABLE_LOGGING
using System.Collections;
using UnityEngine;

namespace Scripts.Framework.Service
{
	public abstract class SRSceneServiceBase<T, TImpl> : SRServiceBase<T>, IAsyncService
		where T : class
		where TImpl : Component
	{

#if ENABLE_LOGGING
		private const bool Logging = true;
#else
		private const bool Logging = false;
#endif

#if UNITY_PRO_LICENSE
		private const bool IsPro = true;
#else
		private const bool IsPro = false;
#endif

		/// <summary>
		/// Name of the scene this service's contents are within
		/// </summary>
		protected abstract string SceneName { get; }

		/// <summary>
		/// Scene contents root object
		/// </summary>
		protected TImpl RootObject
		{
			get { return _rootObject; }
		}

		public bool IsLoaded { get { return _rootObject != null; } }

		private TImpl _rootObject;

		protected override void Start()
		{

			base.Start();

			StartCoroutine(LoadCoroutine());

		}

		protected override void OnDestroy()
		{

			if(IsLoaded)
				Destroy(_rootObject.gameObject);

			base.OnDestroy();
		}

		protected virtual void OnLoaded() {}

		private IEnumerator LoadCoroutine()
		{

			if (_rootObject != null)
				yield break;

			SRServiceManager.LoadingCount++;

			if (Logging)
				Debug.Log("[Service] Loading scene ({0})".Fmt(SceneName), this);

			if(IsPro)
				yield return Application.LoadLevelAdditiveAsync(SceneName);
			else
				Application.LoadLevelAdditive(SceneName);

			if (Logging)
				Debug.Log("[Service] Scene loaded. Searching for root object...", this);

			var go = GameObject.Find(SceneName);

			if (go == null)
				goto Error;

			var timpl = go.GetComponent<TImpl>();

			if (timpl == null)
				goto Error;

			_rootObject = timpl;
			_rootObject.transform.parent = CachedTransform;

			DontDestroyOnLoad(go);

			Debug.Log("[Service] Loading {0} complete. (Scene: {1})".Fmt(GetType().Name, SceneName), this);
			SRServiceManager.LoadingCount--;

			OnLoaded();

			yield break;

			Error:

			SRServiceManager.LoadingCount--;
			Debug.LogError("[Service] Root object ({0}) not found".Fmt(SceneName), this);
			enabled = false;

		}

	}
}