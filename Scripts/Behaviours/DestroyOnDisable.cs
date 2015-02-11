using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	[AddComponentMenu(ComponentMenuPaths.DestroyOnDisable)]
	public class DestroyOnDisable : MonoBehaviour
	{

		private void OnDisable()
		{
			Destroy(gameObject);
		}

	}


}