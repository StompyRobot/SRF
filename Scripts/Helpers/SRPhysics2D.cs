using UnityEngine;
using System.Collections;

public static class SRPhysics2D 
{

	private static readonly Collider2D[] ColliderCache = new Collider2D[1024];

	public static int OverlapPoint<T>(Vector2 point, int layerMask, ref T[] results) where T : Component
	{

		var c = Physics2D.OverlapPointNonAlloc(point, ColliderCache, layerMask);

		if (c == 0)
			return 0;

		var count = 0;

		for (var i = 0; i < c; i++) {

			var collider = ColliderCache[i];

			var t = collider.GetComponent<T>();

			if(t == null)
				continue;

			if (results.Length <= count) {
				Debug.LogWarning("Truncating results - result array insufficiant length");
				break;
			}

			results[count] = t;
			count += 1;

		}

		return count;

	}

}
