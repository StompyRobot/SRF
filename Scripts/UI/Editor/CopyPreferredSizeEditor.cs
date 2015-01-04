#if ENABLE_4_6_FEATURES

using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace SRF.UI.Editor
{

	[CustomEditor(typeof(CopyPreferredSize))]
	[CanEditMultipleObjects]
	public class CopyPreferredSizeEditor : UnityEditor.Editor
	{

		private SerializedProperty _copySourceProperty;

		private SerializedProperty _paddingWidthProperty;
		private SerializedProperty _paddingHeightProperty;

		protected void OnEnable()
		{

			this._paddingWidthProperty = this.serializedObject.FindProperty("PaddingWidth");
			this._paddingHeightProperty = this.serializedObject.FindProperty("PaddingHeight");
			this._copySourceProperty = this.serializedObject.FindProperty("CopySource");

		}

		public override void OnInspectorGUI()
		{

			//base.OnInspectorGUI();

			EditorGUILayout.Space();

			EditorGUILayout.PropertyField(this._copySourceProperty);
			EditorGUILayout.PropertyField(this._paddingWidthProperty);
			EditorGUILayout.PropertyField(this._paddingHeightProperty);
			this.serializedObject.ApplyModifiedProperties();

			this.serializedObject.Update();

		}

	}


}

#endif