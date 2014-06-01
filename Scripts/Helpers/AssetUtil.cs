using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;

namespace SRF.Helpers
{
	public static class AssetUtil
	{

#if UNITY_EDITOR

		// Copied from FingerGestures

		/// <summary>
		//	This makes it easy to create, name and place unique new ScriptableObject asset files.
		/// </summary>
		public static T CreateAsset<T>() where T : ScriptableObject
		{
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);

			if (path == "")
				path = "Assets";
			else if (Path.GetExtension(path) != "")
				path = path.Replace(Path.GetFileName(path), "");

			return CreateAsset<T>(path, "New " + typeof (T).Name);
		}

		public static T CreateAsset<T>(string path, string name) where T : ScriptableObject
		{
			if (string.IsNullOrEmpty(path))
				path = "Assets";

			if (!name.EndsWith(".asset"))
				name += ".asset";

			string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name);

			T asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset(asset, assetPathAndName);
			AssetDatabase.SaveAssets();

			return asset;
		}

		public static void SelectAssetInProjectView(Object asset)
		{
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

#endif

	}
}