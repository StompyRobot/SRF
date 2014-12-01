#if ENABLE_4_6_FEATURES

using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SRF.UI.Editor
{


	[CustomEditor(typeof (StyleComponent))]
	[CanEditMultipleObjects]
	public class StyleComponentEditor : UnityEditor.Editor
	{

		public override void OnInspectorGUI()
		{
			
			base.OnInspectorGUI();

			var styleComponent = target as StyleComponent;

			if (styleComponent == null) {
				Debug.LogWarning("Target is null, expected StyleComponent");
				return;
			}

			var styleRoot = styleComponent.GetComponentInParent<StyleRoot>();

			if (styleRoot == null) {

				EditorGUILayout.HelpBox("There must be a StyleRoot component above this object in the hierarchy.", MessageType.Error,
					true);

				return;

			}

			var styleSheet = styleRoot.StyleSheet;

			if (styleSheet == null) {
				
				EditorGUILayout.HelpBox("Style Root has no stylesheet set.", MessageType.Warning);

				EditorGUILayout.Popup("Key", 0,
					new [] {string.IsNullOrEmpty(styleComponent.StyleKey) ? "--" : styleComponent.StyleKey});

				return;

			}

			var options = styleRoot.StyleSheet.GetStyleKeys(true).ToList();

			var index = options.IndexOf(styleComponent.StyleKey) + 1;

			options.Insert(0, "--");

			EditorGUILayout.Separator();

			var newIndex = EditorGUILayout.Popup("Key", index, options.ToArray());

			if (newIndex != index) {

				if (newIndex == 0)
					styleComponent.StyleKey = "";
				else
					styleComponent.StyleKey = options[newIndex];

				EditorUtility.SetDirty(styleComponent);
				styleComponent.Refresh();

			}

			EditorGUILayout.Separator();

			GUILayout.BeginHorizontal();

			if (GUILayout.Button("Open StyleSheet"))
				Selection.activeObject = styleRoot.StyleSheet;

			EditorGUILayout.Separator();

			if (GUILayout.Button("Select StyleRoot"))
				Selection.activeGameObject = styleRoot.gameObject;

			GUILayout.EndHorizontal();

			EditorGUILayout.Separator();

		}

	}

}

#endif
