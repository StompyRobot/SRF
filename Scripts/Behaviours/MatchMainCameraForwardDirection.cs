using UnityEngine;

namespace Scripts.Visual
{
	public class MatchMainCameraForwardDirection : SRMonoBehaviour
	{

		public bool CheckEveryFrame = false;

		private Transform _target;

		void Start()
		{

			_target = Camera.main.transform;

		}

		void Update()
		{

			if (CheckEveryFrame)
				_target = Camera.main.transform;

			CachedTransform.forward = _target.forward;

		}

	}
}
