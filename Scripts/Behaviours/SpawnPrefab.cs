using UnityEngine;
using System.Collections;

/// <summary>
/// Spawn a prefab and add it as a child to this object (useful for nesting prefabs)
/// </summary>
public class SpawnPrefab : SRMonoBehaviour
{

	public GameObject Prefab;

	public bool ResetLocals = true;

	void Start()
	{

		var o = (GameObject)Instantiate(Prefab, CachedTransform.position, CachedTransform.rotation);
		o.transform.parent = CachedTransform;

		if (ResetLocals) {
			o.transform.SetLocals(Prefab.transform);
		}

		Destroy(this);

	}

}
