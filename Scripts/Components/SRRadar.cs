using UnityEngine;

namespace SRF.Components
{

	public abstract class SRRadar<T> : SRRadarBase<T> where T : class, IHasTransform
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

}