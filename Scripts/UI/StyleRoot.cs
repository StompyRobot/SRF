#if ENABLE_4_6_FEATURES

using UnityEngine;

namespace SRF.UI
{

	[ExecuteInEditMode]
	public sealed class StyleRoot : SRMonoBehaviour
	{

		public StyleSheet StyleSheet;

		private StyleSheet _activeStyleSheet;

		public Style GetStyle(string key)
		{

			if (StyleSheet == null) {

				Debug.LogWarning("[StyleRoot] StyleSheet is not set.", this);
				return null;

			}

			return StyleSheet.GetStyle(key);

		}

		private void OnEnable()
		{

			_activeStyleSheet = null;

			if(StyleSheet != null)
				OnStyleSheetChanged();

		}

		private void OnDisable()
		{
			OnStyleSheetChanged();
		}

		private void Update()
		{

			if (_activeStyleSheet != StyleSheet) {
			 	OnStyleSheetChanged();
			}

		}

		private void OnStyleSheetChanged()
		{

			_activeStyleSheet = StyleSheet;

			BroadcastMessage("SRStyleDirty", SendMessageOptions.DontRequireReceiver);

		}

		public void SetDirty()
		{
			_activeStyleSheet = null;
		}

	}

}

#endif
