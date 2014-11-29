#if ENABLE_4_6_FEATURES

using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{

	/// <summary>
	/// Adds a LayoutDirty callback to the default Text component.
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.SRText)]
	public class SRText : Text
	{

		public event Action<SRText> LayoutDirty;

		public override void SetLayoutDirty()
		{

			base.SetLayoutDirty();

			if (LayoutDirty != null)
				LayoutDirty(this);

		}

	}
}

#endif