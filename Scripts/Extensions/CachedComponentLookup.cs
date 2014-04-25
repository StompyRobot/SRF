using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class CachedComponentLookup
{

	public static TComp GetComponentCached<TComp>(this GameObject go) where TComp : MonoBehaviour
	{
		return CachedComponentLookup<TComp>.GetComponent(go);
	}

}

public static class CachedComponentLookup<T> where T : MonoBehaviour
{

	private static Dictionary<int, T> _goLookup = new Dictionary<int,T>();

	/// <summary>
	/// Lookup a component on a gameobject, using the cache if possible.
	/// </summary>
	/// <param name="go"></param>
	/// <returns></returns>
	public static T GetComponent(GameObject go)
	{

		var id = go.GetInstanceID();

		T component;

		if (!_goLookup.TryGetValue(id, out component)) {
			component = go.GetComponent<T>();
			_goLookup.Add(id, component);
		}

		return component;

	}

}
