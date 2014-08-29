#if ENABLE_UNITYEVENTS

using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
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