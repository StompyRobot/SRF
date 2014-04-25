using UnityEngine;

namespace Scripts.Framework.Helpers
{
	public static class SRTouchUtils
	{

		public static void CameraDragStart(SRInputCameraBehaviour camera, ref Vector3 touchWorldPos, Vector2 touchScreenPos)
		{
			
			// actual position under the touch
			touchWorldPos = camera.ScreenPosToGamePlane(touchScreenPos, true);
			camera.MoveStart();

		}

		public static void CameraDrag(SRInputCameraBehaviour camera, ref Vector3 touchWorldPos, Vector2 touchScreenPos,
			out Vector3 dragVelocity)
		{

			var targetWorldPosition = touchWorldPos; // target position under the touch

			// actual position under the touch
			var currentWorldPosition = camera.ScreenPosToGamePlane(touchScreenPos, true);

			var diff = targetWorldPosition - currentWorldPosition;

			camera.MoveDiff(diff);

			dragVelocity = diff / RealTime.deltaTime;

			touchWorldPos = camera.ScreenPosToGamePlane(touchScreenPos, true);

		}

		public static void CameraDragEnd(SRInputCameraBehaviour camera, ref Vector3 dragVelocity)
		{

			camera.MoveComplete();
			camera.Velocity = dragVelocity;

		}

	}
}
