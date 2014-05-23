using UnityEngine;

namespace SRF.Behaviours
{

	public class ScrollTexture : MonoBehaviour
	{

		public int Material;
		public string TextureName = "_MainTex";

		public Vector2 ScrollSpeed;

		public bool Shared = true;

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

			var dt = Time.time;

			m.SetTextureOffset(TextureName, ScrollSpeed*dt);

		}

	}

}
