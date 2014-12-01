using System.Runtime.InteropServices;
#if ENABLE_4_6_FEATURES
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace SRF.UI
{

	[RequireComponent(typeof(Graphic))]
	[ExecuteInEditMode]
	public class StyleComponent : SRMonoBehaviour
	{

		public bool IgnoreImage = false;

		[HideInInspector]
		public string StyleKey;

		private Style _activeStyle;
		private Graphic _graphic;
		private Button _button;
		private Image _image;

		private void Start()
		{
			
			Refresh();

		}

#if UNITY_EDITOR

		private void Update()
		{
			
			ApplyStyle();

		}

#endif

		public void Refresh()
		{

			if (string.IsNullOrEmpty(StyleKey)) {

				_activeStyle = null;
				return;

			}

			var styleRoot = GetComponentInParent<StyleRoot>();

			if (styleRoot == null) {

				Debug.LogWarning("[StyleComponent] No StyleRoot object found in parents.", this);
				_activeStyle = null;
				return;

			}

			var s = styleRoot.GetStyle(StyleKey);

			if (s == null) {

				Debug.LogWarning("[StyleComponent] Style not found ({0})".Fmt(StyleKey), this);
				_activeStyle = null;
				return;

			}

			_activeStyle = s;
			ApplyStyle();

		}

		private void ApplyStyle()
		{

			if (_activeStyle == null)
				return;

			if(_graphic == null)
				_graphic = GetComponent<Graphic>();

			if(_button == null)
				_button = GetComponent<Button>();

			if (_image == null)
				_image = GetComponent<Image>();

			if (!IgnoreImage && _image != null) {
				_image.sprite = _activeStyle.Image;
			}

			if (_button != null) {

				var colours = new ColorBlock();
				colours.normalColor = _activeStyle.NormalColor;
				colours.highlightedColor = _activeStyle.HoverColor;
				colours.pressedColor = _activeStyle.ActiveColor;
				colours.disabledColor = _activeStyle.DisabledColor;
				colours.colorMultiplier = 1f;

				_button.colors = colours;
				_graphic.color = Color.white;

			} else {

				_graphic.color = _activeStyle.NormalColor;

			}

		}

		private void SRStyleDirty()
		{

			Debug.Log("Style Dirty");
			Refresh();

		}

	}

}

#endif