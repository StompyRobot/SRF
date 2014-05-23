using UnityEngine;

namespace SRF.Camera
{
	[RequireComponent(typeof(UnityEngine.Camera))]
	[ExecuteInEditMode]
	public class MatchOrthoSize : SRMonoBehaviourEx
	{

		[RequiredField]
		public UnityEngine.Camera Target;

		private UnityEngine.Camera _this;

		void LateUpdate()
		{

			if (_this == null) {

				_this = GetComponent<UnityEngine.Camera>();

			}

			if (_this == null || Target == null)
				return;

			_this.orthographicSize = Target.orthographicSize;

		}

	}
}
