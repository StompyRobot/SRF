using UnityEngine;
using System.Collections;

namespace SRF.Behaviours
{

	[AddComponentMenu(Internal.ComponentMenuPaths.DestroyOnDisable)]
	public class DestroyOnDisable : MonoBehaviour
	{

		private void OnDisable()
		{
			Destroy(this.gameObject);
		}

	}


}