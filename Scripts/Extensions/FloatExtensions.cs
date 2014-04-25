using UnityEngine;
using System.Collections;

public static class FloatExtensions
{

	public static float Sqr(this float f)
	{
		return f * f;
	}

	public static bool ApproxZero(this float f)
	{
		return Mathf.Approximately(0, f);
	}

	public static bool Approx(this float f, float f2)
	{
		return Mathf.Approximately(f, f2);
	}

}
