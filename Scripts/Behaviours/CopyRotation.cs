using UnityEngine;

namespace Scripts.Framework.Behaviours
{
	public class CopyRotation : SRMonoBehaviour
	{

		public Transform Target;

		public bool CopyX = true;
		public bool CopyY = true;
		public bool CopyZ = true;

		public Vector3 Offset = Vector3.zero;

		void LateUpdate()
		{

			var current = CachedTransform.rotation.eulerAngles;

			var rot = Target.rotation.eulerAngles;

			rot += Offset;

			if (!CopyX)
				rot.x = current.x;

			if (!CopyY)
				rot.y = current.y;

			if (!CopyZ)
				rot.z = current.z;

			CachedTransform.rotation = Quaternion.Euler(rot);

		}

	}
}
