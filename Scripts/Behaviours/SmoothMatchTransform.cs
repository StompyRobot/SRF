using UnityEngine;
using System.Collections;

namespace SRF.Behaviours
{

	/// <summary>
	/// Smoothly interpolate position/rotation/scale to match another transform
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.SmoothMatchTransform)]
	public class SmoothMatchTransform : SRMonoBehaviour
	{

		public Transform Target;

		public bool MatchPosition = true;
		public bool MatchRotation = true;
		public bool MatchScale = true;

		public float SmoothStrength = 15f;

		public bool IgnoreTimeScale = false;

		private Vector3 _positionVelocity;
		private Vector3 _scaleVelocity;

		private void Update()
		{

			if (Target == null)
				return;

			var dt = IgnoreTimeScale ? RealTime.deltaTime : Time.deltaTime;

			if (MatchPosition)
				CachedTransform.position = Vector3.SmoothDamp(CachedTransform.position, Target.position, ref _positionVelocity,
					SmoothStrength,
					float.MaxValue, dt);

			// TODO: Implement rotation smoothing
			/*if(MatchRotation)
				CachedTransform.rotation = NGUIMath.SpringLerp(CachedTransform.rotation, Target.rotation, SmoothStrength, dt);*/

			if (MatchScale)
				CachedTransform.localScale = Vector3.SmoothDamp(CachedTransform.localScale, Target.localScale, ref _scaleVelocity,
					SmoothStrength,
					float.MaxValue, dt);

		}

	}

}