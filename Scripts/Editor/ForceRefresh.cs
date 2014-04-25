using System.Linq;
using UnityEngine;
using UnityEditor;

public class ForceRefresh : ScriptableObject
{
	[MenuItem("Assets/Force Refresh")]
	static void DoIt()
	{

		var assets =
			Selection.GetFiltered(typeof (Object), SelectionMode.DeepAssets)
			         .Where(p => p is Material || p is GameObject)
			         .ToArray();


		if (!EditorUtility.DisplayDialog("Confirm", "Are you sure you want to refresh {0} assets?".Fmt(assets.Length), "OK",
		                                "Cancel")) {
			Debug.Log("Aborted");
		}

		for (int i = 0; i < assets.Length; i++) {

			var asset = assets[i];
			var assetPath = AssetDatabase.GetAssetPath(asset);

			var message = "Refreshing {0} of {1} ({2})".Fmt(i+1, assets.Length, assetPath);

			EditorUtility.DisplayProgressBar("Refreshing Assets",
			                                 message,
			                                 (float) i/assets.Length);
			
			Debug.Log(message, asset);
			AssetDatabase.ImportAsset(assetPath,
			                          ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate |
			                          ImportAssetOptions.DontDownloadFromCacheServer);
			EditorUtility.SetDirty(asset);

		}

		EditorUtility.DisplayProgressBar("Refreshing Assets", "Refresh Asset Database", 1.0f);

		AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
		AssetDatabase.SaveAssets();

		EditorUtility.ClearProgressBar();

	}
}