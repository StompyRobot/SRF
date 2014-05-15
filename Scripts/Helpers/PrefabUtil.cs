using System.Collections.Generic;
using UnityEngine;

namespace SRF.Helpers
{
	public static class PrefabUtil
	{

		public static IList<T> FindPrefabs<T>() where T : Component
		{

#if !UNITY_EDITOR

			Debug.LogError("FindPrefabs() can not be used at runtime (Editor Only)")
			return new List<T>();
#else

			var assetPaths = UnityEditor.AssetDatabase.GetAllAssetPaths();
			var msg = "Searching for prefabs with {0}".Fmt(typeof (T).Name);
			var result = new List<T>();

			for (int i = 0; i < assetPaths.Length; i++) {

				var assetPath = assetPaths[i];

				UnityEditor.EditorUtility.DisplayProgressBar("Searching", msg, i/(float) assetPaths.Length);

				var a = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof (GameObject)) as GameObject;

				if (a == null)
					continue;

				var t = a.GetComponent<T>();

				if(t == null)
					continue;

				result.Add(t);

			}

			UnityEditor.EditorUtility.ClearProgressBar();
			return result;

#endif

		}

	}
}