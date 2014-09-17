using System;
using System.IO;
using UnityEngine;
using UnityEditor;

public class SRScreenshotManager : EditorWindow
{

	public int ScreenshotSize = 4;
	public string ScreenshotPath = Environment.CurrentDirectory;
	public string ScreenshotName = "Screenshot";


	[MenuItem("Window/SRScreenshotManager")]
	static void Open()
	{
		GetWindow<SRScreenshotManager>("SRScreenshotManager", true);
		//GetWindowWithRect<SRScreenshotManager>(new Rect(10, 10, 290, 100), false, "SRScreenshotManager", true);
	}

	void Take()
	{

		var filePath = Path.Combine(ScreenshotPath, "{0}.png".Fmt(ScreenshotName));
		var i = 0;

		while (File.Exists(filePath)) {

			++i;

			filePath = Path.Combine(ScreenshotPath, "{0}{1}.png".Fmt(ScreenshotName, i));

		}

		Debug.Log("[SRScreenshotManager] Taking screenshot. Saving to {0}".Fmt(filePath));

		Application.CaptureScreenshot(filePath, ScreenshotSize);

		Debug.Log("[SRScreenshotManager] Done");

	}

	void OnGUI()
	{


		GUILayout.Label("Settings", EditorStyles.boldLabel);

		EditorGUILayout.BeginVertical();

		ScreenshotSize = EditorGUILayout.IntSlider("Size Multiplier", ScreenshotSize, 0, 6);
		ScreenshotPath = EditorGUILayout.TextField("Save Path", ScreenshotPath);

		EditorGUILayout.EndVertical();

		EditorGUILayout.Separator();

		GUI.enabled = Application.isPlaying;

		if (GUILayout.Button("Take Screenshot"))
			Take();

		GUI.enabled = true;

	}

}