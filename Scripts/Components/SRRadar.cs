using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SRRadar<T> : SRRadarBase<T> where T : SRMonoBehaviour
{

	protected override void PerformScan()
	{
		
		var nearby = Physics.OverlapSphere(CachedTransform.position, Range, Mask);

		if (nearby.Length == 0) {
			return;
		}

		for (int i = 0; i < nearby.Length; i++) {

			var n = nearby[i];
			var go = n.gameObject;

			HandleDiscoveredObject(go);
	
		}

	}

}
