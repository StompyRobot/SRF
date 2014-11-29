using UnityEngine;

namespace SRF.Behaviours
{

	/// <summary>
	/// Scrolls the material texture UVs
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.ScrollTexture)]
	public class ScrollTexture : MonoBehaviour
	{

		public int Material;
		public string TextureName = "_MainTex";

		public Vector2 ScrollSpeed;

		public bool Shared = true;
		public bool IgnoreTimeScale = false;

		private Renderer _renderer;

		void Update()
		{

			if (_renderer == null)
				_renderer = renderer;

			Material m;

			if (Shared)
				m = _renderer.sharedMaterials[Material];
			else
				m = _renderer.materials[Material];

			var dt = IgnoreTimeScale ? RealTime.time : Time.time;

			m.SetTextureOffset(TextureName, ScrollSpeed*dt);

		}

	}

}
