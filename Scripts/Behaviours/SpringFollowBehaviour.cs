using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	[AddComponentMenu(ComponentMenuPaths.SpringFollow)]
	public class SpringFollowBehaviour : SRMonoBehaviourEx
	{

		public Transform Target;

		public Vector3 TargetPosition;

		public bool UseLateUpdate = false;

		public float Time = 0.1f;
		public float MaxSpeed = 20f;

		private Vector3 _currentVelocity;

		void DoUpdate()
		{

			if (Target != null)
				TargetPosition = Target.position;

			CachedTransform.position = Vector3.SmoothDamp(CachedTransform.position, TargetPosition, ref _currentVelocity, Time,
				MaxSpeed, UnityEngine.Time.unscaledDeltaTime);

		}

		protected override void Update()
		{

			if(!UseLateUpdate)
				DoUpdate();

		}

		protected void LateUpdate()
		{

			if(UseLateUpdate)
				DoUpdate();

		}

		[ContextMenu("Set To Current Position")]
		public void SetToCurrentPosition()
		{

			TargetPosition = CachedTransform.position;

		}


	}
}