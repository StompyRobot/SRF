#if ENABLE_4_6_FEATURES
using System;
using UnityEngine;

namespace SRF.UI
{

	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	public class ResponsiveEnable : ResponsiveBase
	{

		[Serializable]
		public struct Entry
		{

			public Modes Mode;

			public float ThresholdWidth;
			public float ThresholdHeight;

			public GameObject GameObject;

		}

		public enum Modes
		{

			EnableAbove,
			EnableBelow

		}

		public Entry[] Entries = new Entry[0];

		protected override void Refresh()
		{

			var rect = RectTransform.rect;

			for (var i = 0; i < Entries.Length; i++) {

				var e = Entries[i];

				if (e.GameObject == null)
					continue;

				var enable = true;

				switch (e.Mode) {

					case Modes.EnableAbove: {

							if (e.ThresholdHeight > 0)
								enable = rect.height >= e.ThresholdHeight && enable;

							if (e.ThresholdWidth > 0)
								enable = rect.width >= e.ThresholdWidth && enable;

							break;

						}
					case Modes.EnableBelow: {

							if (e.ThresholdHeight > 0)
								enable = rect.height <= e.ThresholdHeight && enable;

							if (e.ThresholdWidth > 0)
								enable = rect.width <= e.ThresholdWidth && enable;

							break;
						}
					default:
						throw new IndexOutOfRangeException();

				}

				e.GameObject.SetActive(enable);

			}

		}

	}

} 
#endif
