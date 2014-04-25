using UnityEngine;
using System.Collections;

public static class Coroutines
{

	public static IEnumerator WaitForSecondsRealTime(float time)
	{

		var endTime = Time.realtimeSinceStartup + time;

		while (Time.realtimeSinceStartup < endTime)
			yield return null;

	}

}
