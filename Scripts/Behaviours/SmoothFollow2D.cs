using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	/// <summary>
	/// Smoothly follow the target transform, on the x and y axis only.
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.SmoothFollow2D)]
	public class SmoothFollow2D : SRMonoBehaviourEx
	{

		[RequiredField(AutoSearch = false)]
		public Transform Target;

		public float SmoothTime = 0.3f;

		private float _velocityX;
		private float _velocityY;

		protected override void Update()
		{

			base.Update();

			var pos = CachedTransform.position;

			pos.x = Mathf.SmoothDamp(pos.x,
				Target.position.x, ref _velocityX, SmoothTime, 10000f, Time.deltaTime);

			pos.y = Mathf.SmoothDamp(pos.y,
				Target.position.y, ref _velocityY, SmoothTime, 10000f, Time.deltaTime);

			CachedTransform.position = pos;

		}

	}
}
