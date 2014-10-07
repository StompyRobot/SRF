using UnityEngine;

namespace SRF.Behaviours
{
	public class SmoothOscillateBehaviour : SRMonoBehaviour
	{

		public Vector3 Intensity;

		public float Speed = 1f;

		public bool IgnoreTimeScale = false;

		private Vector3 _defaultLocalPosition;

		private float _time;

		protected void Start()
		{

			_defaultLocalPosition = CachedTransform.localPosition;

		}

		protected void Update()
		{

			_time += (IgnoreTimeScale ? RealTime.deltaTime : Time.deltaTime) * Speed;

			CachedTransform.localPosition = _defaultLocalPosition +
			                                Intensity*Mathf.Sin(_time);

		}

	}
}
