#if ENABLE_4_6_FEATURES

using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{

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

			var styleRoot = GetStyleRoot();

			if (styleRoot == null) {

				Debug.LogWarning("[StyleComponent] No active StyleRoot object found in parents.", this);
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

		/// <summary>
		/// Find the nearest enable style root component in parents
		/// </summary>
		/// <returns></returns>
		private StyleRoot GetStyleRoot()
		{

			Transform t = CachedTransform;
			StyleRoot root;

			var i = 0;

			do {

				root = t.GetComponentInParent<StyleRoot>();

				if (root != null)
					t = root.transform.parent;

				++i;

				if (i > 100) {
					Debug.LogWarning("Breaking Loop");
					break;
				}

			} while ((root != null && !root.enabled) && t != null);

			return root;

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

				var colours = _button.colors;
				colours.normalColor = _activeStyle.NormalColor;
				colours.highlightedColor = _activeStyle.HoverColor;
				colours.pressedColor = _activeStyle.ActiveColor;
				colours.disabledColor = _activeStyle.DisabledColor;
				colours.colorMultiplier = 1f;

				_button.colors = colours;

				if(_graphic != null)
					_graphic.color = Color.white;

			} else {

				_graphic.color = _activeStyle.NormalColor;

			}

		}

		private void SRStyleDirty()
		{
			Refresh();
		}

	}

}

#endif
