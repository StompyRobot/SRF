using UnityEngine;

namespace SRF.Behaviours
{

	[AddComponentMenu(Internal.ComponentMenuPaths.DontDestroyOnLoad)]
	public class DontDestroyOnLoad : MonoBehaviour
	{

		void Awake()
		{
			DontDestroyOnLoad(this.gameObject);
		}

	}

}