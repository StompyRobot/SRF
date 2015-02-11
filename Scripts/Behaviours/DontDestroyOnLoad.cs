using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	[AddComponentMenu(ComponentMenuPaths.DontDestroyOnLoad)]
	public class DontDestroyOnLoad : MonoBehaviour
	{

		void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

	}

}