using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	/// <summary>
	/// Smoothly floats the attached transform on the x and y axis based on a smoothing value.
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.SmoothFloatBehaviour)]
	public class SmoothFloatBehaviour : SRMonoBehaviour
	{

		public Vector2 FloatLimits;

		public float FloatSpeed = 3;
		public float Smooth = 3.5f;

		public float TimeOffset;
		public bool IgnoreTimeScale = true;

		public bool SeedTime = false;

		private Vector3 _targetPosition;
		private Vector3 _startPosition;

		private float _time;

		private void Start()
		{

			_startPosition = CachedTransform.localPosition;

			if (SeedTime)
				_time = Time.realtimeSinceStartup * Random.value;

			_time += TimeOffset;

		}

		private void Update()
		{

			var t = IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;

			_time += t;

			_targetPosition = _startPosition;
			_targetPosition.x += FloatLimits.x*Mathf.Sin(_time)*FloatSpeed;
			_targetPosition.y += FloatLimits.y*Mathf.Cos(_time)*FloatSpeed;

			CachedTransform.localPosition = Vector3.Lerp(CachedTransform.localPosition, _targetPosition,
				Smooth*t);

		}

	}

}