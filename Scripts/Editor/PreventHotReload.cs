using System;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class PreventHotLoad : EditorWindow
{

	static PreventHotLoad()
	{
		EditorApplication.update += Update;
	}

	private static void Update()
	{

		if (EditorApplication.isCompiling && EditorApplication.isPlaying) {

			Debug.Log("[PreventHotReload] Code reload in progress. Ending play mode.");
			EditorApplication.isPlaying = false;

		}

	}

}