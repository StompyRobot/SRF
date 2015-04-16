﻿using UnityEngine;

namespace SRF.Components
{

	public abstract class SRRadar2D<T> : SRRadarBase<T> where T : class, IHasTransform
	{

		private static Collider2D[] ColliderCache;

		protected override void PerformScan()
		{

			if (ColliderCache == null)
				ColliderCache = new Collider2D[512];

			var count = Physics2D.OverlapCircleNonAlloc(CachedTransform.position, Range, ColliderCache, Mask);

			if (count == 0)
				return;

			for (int i = 0; i < count; i++) {

				var n = ColliderCache[i];
				var go = n.gameObject;

				HandleDiscoveredObject(go);

			}

		}

	}

}