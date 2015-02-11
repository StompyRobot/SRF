using UnityEngine;

/// <summary>
/// Nicer debug rendering colours
/// </summary>
public static class SRColors
{

	public static readonly Color Green = new Color32(127, 201, 122, 255);
	public static readonly Color Blue = new Color32(124, 140, 185, 255);
	public static readonly Color Red = new Color32(203, 130, 126, 255);
	public static readonly Color Yellow = new Color32(201, 201, 126, 255);


	public static Color Average(Color c1, Color c2)
	{

		return new Color(
			SRMath.Average(c1.r, c2.r),
			SRMath.Average(c1.g, c2.g),
			SRMath.Average(c1.b, c2.b),
			SRMath.Average(c1.a, c2.a)
			);

	}


}
