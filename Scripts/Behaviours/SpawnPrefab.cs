using UnityEngine;

namespace SRF.Behaviours
{

	/// <summary>
	/// Spawn a prefab and add it as a child to this object (useful for nesting prefabs)
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.SpawnPrefab)]
	public class SpawnPrefab : SRMonoBehaviour
	{

		public GameObject Prefab;

		public bool ResetLocals = true;

		private void Start()
		{

			var o = (GameObject) Instantiate(Prefab, CachedTransform.position, CachedTransform.rotation);
			o.transform.parent = CachedTransform;

			if (ResetLocals) {
				o.transform.SetLocals(Prefab.transform);
			}

			Destroy(this);

		}

	}

}