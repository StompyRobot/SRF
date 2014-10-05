using UnityEngine;

namespace SRF.Camera
{

	[ExecuteInEditMode]
	[RequireComponent(typeof(UnityEngine.Camera))]
	public class MatchCamera : SRMonoBehaviourEx
	{

		[RequiredField]
		public UnityEngine.Camera TargetCamera;

		private UnityEngine.Camera _camera;

		protected override void Update()
		{

			base.Update();

			if (_camera == null)
				_camera = GetComponent<UnityEngine.Camera>();

			if (TargetCamera == null) {

				if (Application.isPlaying) {

					Debug.LogError("TargetCamera property not set", this);
					enabled = false;

				}

				return;

			}

			_camera.isOrthoGraphic = TargetCamera.isOrthoGraphic;
			_camera.orthographicSize = TargetCamera.orthographicSize;
			_camera.fieldOfView = TargetCamera.fieldOfView;
			_camera.farClipPlane = TargetCamera.farClipPlane;
			_camera.nearClipPlane = TargetCamera.nearClipPlane;

		}

	}
}