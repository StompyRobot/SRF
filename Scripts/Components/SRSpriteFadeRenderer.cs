using SRF.Internal;
using UnityEngine;

namespace SRF.Components
{

	/// <summary>
	/// Handles fading the alpha on a sprite
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.SRSpriteFadeRenderer)]
	public class SRSpriteFadeRenderer : SRFadeRenderer
	{

		public SpriteRenderer SpriteRenderer;

		void Start()
		{
			EditorAssertNotNull(SpriteRenderer, "SpriteRenderer");
		}

		void Update()
		{

			if(SpriteRenderer != null)
				SpriteRenderer.color = SpriteRenderer.color.Alpha(Alpha);

		}

	}

}
