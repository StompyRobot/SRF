using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	[AddComponentMenu(ComponentMenuPaths.MatchMainCameraForwardDirection)]
	public class MatchMainCameraForwardDirection : SRMonoBehaviour
	{

		public bool CheckEveryFrame = false;

		private Transform _target;

		void Start()
		{

			_target = UnityEngine.Camera.main.transform;

		}

		void Update()
		{

			if (CheckEveryFrame)
				_target = UnityEngine.Camera.main.transform;

			CachedTransform.forward = _target.forward;

		}

	}
}
