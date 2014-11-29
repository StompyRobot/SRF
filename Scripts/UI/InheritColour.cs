#if ENABLE_4_6_FEATURES

using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{

	[RequireComponent(typeof(Graphic))]
	[ExecuteInEditMode]
	[AddComponentMenu(Internal.ComponentMenuPaths.InheritColour)]
	public class InheritColour : SRMonoBehaviour
	{

		public Graphic From;

		private Graphic Graphic
		{
			get
			{

				if (_graphic == null)
					_graphic = GetComponent<Graphic>();

				return _graphic;

			}
		}

		private Graphic _graphic;

		void Refresh()
		{

			if (From == null)
				return;

			Graphic.color = From.color;

		}

		void Update()
		{
			Refresh();
		}

		void Start()
		{
			Refresh();
		}

	}

}

#endif