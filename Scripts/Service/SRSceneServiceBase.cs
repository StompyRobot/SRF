#define ENABLE_LOGGING
using System.Collections;
using UnityEngine;

namespace Scripts.Framework.Service
{
	public abstract class SRSceneServiceBase<T, TImpl> : SRServiceBase<T>
		where T : class
		where TImpl : MonoBehaviour
	{

#if ENABLE_LOGGING
		private const bool Logging = true;
#else
		private const bool Logging = false;
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

		protected bool IsLoaded { get { return _rootObject != null; } }

		private TImpl _rootObject;

		protected override void Awake()
		{

			base.Awake();

		}

		protected override void Start()
		{

			base.Start();

			StartCoroutine(LoadCoroutine());

		}

		protected virtual void OnSceneLoaded() {}

		private IEnumerator LoadCoroutine()
		{

			if (_rootObject != null)
				yield break;

			if (Logging)
				Debug.Log("[Service] Loading scene ({0})".Fmt(SceneName), this);

			yield return Application.LoadLevelAdditiveAsync(SceneName);

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

			if (Logging)
				Debug.Log("[Service] Complete", this);

			OnSceneLoaded();

			yield break;

			Error:

			Debug.LogError("[Service] Root object ({0}) not found".Fmt(SceneName), this);
			enabled = false;

		}

	}
}