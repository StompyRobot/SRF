using System.Linq;
using SRF;
using UnityEngine;

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
