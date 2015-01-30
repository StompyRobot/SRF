#if ENABLE_4_6_FEATURES
using System;
using SRF.Internal;
using UnityEngine;

namespace SRF.UI
{

	[ExecuteInEditMode]
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu(ComponentMenuPaths.ResponsiveEnable)]
	public class ResponsiveEnable : ResponsiveBase
	{

		[Serializable]
		public struct Entry
		{

			public Modes Mode;

			public float ThresholdWidth;
			public float ThresholdHeight;

			public GameObject[] GameObjects;
			public Behaviour[] Components;

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

				if (e.GameObjects != null) {

					for (var j = 0; j < e.GameObjects.Length; j++) {

						var go = e.GameObjects[j];

						if (go != null)
							go.SetActive(enable);

					}

				}

				if (e.Components != null) {

					for (var j = 0; j < e.Components.Length; j++) {

						var go = e.Components[j];

						if (go != null)
							go.enabled = enable;

					}

				}

			}

		}

	}

} 
#endif
