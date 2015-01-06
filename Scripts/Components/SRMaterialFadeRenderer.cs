using UnityEngine;

namespace SRF.Components
{
	/// <summary>
	/// Handles fading the Alpha on a material. Only uses an instance material if alpha is modified
	/// </summary>
	[AddComponentMenu(Internal.ComponentMenuPaths.SRMaterialFadeRenderer)]
	public class SRMaterialFadeRenderer : SRFadeRenderer
	{

		private Material _defaultMaterial;
		private Material _instanceMaterial;

		private Material _activeMaterial;

		private Renderer _renderer;
		public Renderer Renderer {get { return _renderer; }}

		public string ColorProp = "_Color";

		private Color _defaultColor;

		void Awake()
		{

			_renderer = GetComponent<Renderer>();
			_defaultMaterial = Renderer.sharedMaterial;
			_activeMaterial = _defaultMaterial;

			_defaultColor = _defaultMaterial.GetColor(ColorProp);

		}

		void LateUpdate()
		{

			Alpha = Mathf.Clamp01(Alpha);

			if (Mathf.Approximately(1.0f, Alpha)) {

				Renderer.enabled = true;

				if (_activeMaterial != _defaultMaterial) {

					Renderer.enabled = true;
					Renderer.sharedMaterial = _defaultMaterial;
					_activeMaterial = _defaultMaterial;

				}

			} else if(Mathf.Approximately(0.0f, Alpha)) {

				Renderer.enabled = false;

			} else {

				if (_instanceMaterial == null) {
					_instanceMaterial = new Material(_defaultMaterial);
					_instanceMaterial.name += " (Instance)";
				}


				if (_activeMaterial != _instanceMaterial) {

					Renderer.sharedMaterial = _instanceMaterial;
					_activeMaterial = _instanceMaterial;

				}

				Renderer.enabled = true;
				var c = _defaultColor;
				c.a *= Alpha;
				_instanceMaterial.SetColor(ColorProp, c);

			}

		}

	}
}
