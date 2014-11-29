using UnityEngine;
using System.Collections;

namespace SRF.Components
{

	/// <summary>
	/// Provides a single SRFadeRenderer interface, but will fade multiple renderers behind the scenes (useful for fading
	/// a hierarchy)
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.SRCompositeFadeRenderer)]
	public class SRCompositeFadeRenderer : SRFadeRenderer
	{

		public SRFadeRenderer[] Targets;

		public bool ScanOnStart = true;

		protected void Start()
		{

			if(ScanOnStart)
				ScanNow();

		}

		protected void Update()
		{

			for (var i = 0; i < Targets.Length; i++) {
				Targets[i].Alpha = Alpha;
			}

		}

		[ContextMenu("Scan Now")]
		public void ScanNow()
		{

			Targets = GetComponentsInChildren<SRFadeRenderer>();

		}

	}

}