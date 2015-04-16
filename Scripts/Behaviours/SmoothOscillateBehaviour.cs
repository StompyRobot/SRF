using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{
	[AddComponentMenu(ComponentMenuPaths.SmoothOscillate)]
	public class SmoothOscillateBehaviour : SRMonoBehaviour
	{

		public Vector3 Intensity;

		public float Speed = 1f;

		public bool IgnoreTimeScale = false;

		private Vector3 _defaultLocalPosition;

		private float _time;

		public bool SeedTime = true;

		protected void Start()
		{

			_defaultLocalPosition = CachedTransform.localPosition;

			if (SeedTime)
				_time = Time.realtimeSinceStartup * Random.value;

		}

		protected void Update()
		{

			_time += (IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime) * Speed;

			CachedTransform.localPosition = _defaultLocalPosition +
			                                Intensity*Mathf.Sin(_time);

		}

	}
}
