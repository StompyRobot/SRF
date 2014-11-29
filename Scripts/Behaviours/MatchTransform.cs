using UnityEngine;

namespace SRF.Behaviours
{

	/// <summary>
	/// Match the target transform until it is destroyed.
	/// </summary>
	[ExecuteInEditMode]
	[AddComponentMenu(Internal.ComponentMenuPaths.MatchTransform)]
	public class MatchTransform : SRMonoBehaviour
	{

		public Transform Target;


		public bool ExecuteInEditMode = false;
		public bool MatchRotation = true;
		public bool MatchPosition = true;

		public Vector3 PositionOffset;

		private void LateUpdate()
		{

			if (Target == null)
				return;

			if (Application.isEditor && !Application.isPlaying && !ExecuteInEditMode)
				return;

			if (MatchPosition)
				CachedTransform.position = Target.position + PositionOffset;

			if (MatchRotation)
				CachedTransform.rotation = Target.transform.rotation;

		}

	}

}
