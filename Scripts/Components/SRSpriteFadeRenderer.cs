using UnityEngine;

namespace Scripts.Framework.Components
{

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
