using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	[AddComponentMenu(ComponentMenuPaths.LookAt)]
	public class LookAt : SRMonoBehaviour
	{

		public Transform Target;

		void LateUpdate()
		{

			if (Target != null)
				CachedTransform.LookAt(Target, Vector3.up);

		}
	}
	
}