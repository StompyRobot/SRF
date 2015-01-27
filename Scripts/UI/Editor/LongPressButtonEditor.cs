using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

namespace SRF.UI.Editor
{

	[CustomEditor(typeof(LongPressButton), true)]
	[CanEditMultipleObjects]
	public class LongPressButtonEditor : ButtonEditor
	{

		private SerializedProperty _onLongPressProperty;

		protected override void OnEnable()
		{
			base.OnEnable();
			this._onLongPressProperty = this.serializedObject.FindProperty("_onLongPress");
		}


		public override void OnInspectorGUI()
		{

			base.OnInspectorGUI();

			EditorGUILayout.Space();
			this.serializedObject.Update();
			EditorGUILayout.PropertyField(this._onLongPressProperty);
			this.serializedObject.ApplyModifiedProperties();

		}

	}

}