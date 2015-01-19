using UnityEngine;
using System.Collections;

#if !(UNITY_4_0 || UNITY_4_0_1 || UNITY_4_1 || UNITY_4_2 || UNITY_4_3) && !NGUI

public static class RealTime
{

	public static float deltaTime
	{
		get { return Time.unscaledDeltaTime; }
	}

	public static float time
	{
		get { return Time.unscaledTime; }
	}

}

#endif