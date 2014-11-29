using System.Linq;
using SRF;
using SRF.Helpers;
using UnityEngine;

namespace SRF.Components
{

	/// <summary>
	/// Inherit from this abstract class and provide your own type parameter. 
	/// Then, right click on this component in inspector and click "Refresh List".
	/// It will compile a list of all prefabs that have type T attached and add them to the Prefabs
	/// public field.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class PrefabContainer<T> : SRSingleton<PrefabContainer<T>> where T : MonoBehaviour
	{

		public T[] Prefabs;

#if UNITY_EDITOR

		[ContextMenu("Refresh List")]
		public void RefreshList()
		{

			Prefabs = PrefabUtil.FindPrefabs<T>().ToArray();

		}

#endif

	}

}