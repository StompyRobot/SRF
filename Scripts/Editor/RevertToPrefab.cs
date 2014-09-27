using UnityEditor;
using UnityEngine;

namespace SRF.Editor
{
	public class RevertToPrefab : ScriptableObject
	{
		[MenuItem("Tools/Revert Selected GameObjects to Prefab")]
		static void DoRevert()
		{
			var s = Selection.gameObjects;

			var i = 0;

			foreach (var gameObject in s) {

				if (PrefabUtility.RevertPrefabInstance(gameObject)) {

					++i;

				}

			}

			Debug.Log("Reverted {0} GameObjects to prefab state".Fmt(i));

		}

	}
}