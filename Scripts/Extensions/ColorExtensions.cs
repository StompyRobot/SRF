using UnityEngine;
using System.Collections;

public static class ColorExtensions
{

	/// <summary>
	/// Return this color but with 0 alpha
	/// </summary>
	/// <param name="color"></param>
	/// <returns></returns>
	public static Color ZeroAlpha(this Color color)
	{
		color.a = 0;
		return color;
	}


	/// <summary>
	/// Return this color the alpha component multiplied by a
	/// </summary>
	/// <param name="color"></param>
	/// <param name="a">Multiply input color alpha component by this</param>
	/// <returns></returns>
	public static Color MultAlpha(this Color color, float a)
	{
		color.a *= a;
		return color;
	}

	/// <summary>
	/// Returns the color with the alpha set to a
	/// </summary>
	/// <param name="color"></param>
	/// <param name="a"></param>
	/// <returns></returns>
	public static Color Alpha(this Color color, float a)
	{
		color.a = a;
		return color;
	}

}
