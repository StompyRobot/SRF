using UnityEngine;

public static class Vector3Extensions
{

	public static Vector2 xy(this Vector3 v)
	{
		return new Vector2(v.x, v.y);
	}
	
	public static Vector2 xz(this Vector3 v)
	{
		return new Vector2(v.x, v.z);
	}

	/// <summary>
	/// Flattens the vector to ignore the y axis (x,z gameplay plane)
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public static Vector3 Flatten(this Vector3 v)
	{
		return new Vector3(v.x, 0, v.z);
	}

	public static Vector3 Average(Vector3 one, Vector3 two)
	{

		return (one + two) * 0.5f;

	}

	public static Vector3 Average(Vector3 one, Vector3 two, Vector3 three)
	{

		return (one + two + three)/3f;

	}

	public static Vector3 Average(params Vector3[] vectors)
	{

		Vector3 total = Vector3.zero;

		for (int i = 0; i < vectors.Length; i++) {
			total += vectors[i];
		}

		return total/vectors.Length;

	}

	public static float Angle(Vector3 a1, Vector3 a2, Vector3 normal)
	{

		float angle = Vector3.Angle(a1, a2);

		float sign = Mathf.Sign(Vector3.Dot(normal, Vector3.Cross(a1, a2)));

		return angle*sign;

	}

	public static float DistanceSquared(Vector3 vector1, Vector3 vector2)
	{
		return (vector2 - vector1).sqrMagnitude;
	}

	public static Vector3 DirectionTo(this Vector3 t, Vector3 target)
	{
		var diff = target - t;
		diff.Normalize();
		return diff;
	}

	public static Vector3 Multiply(Vector3 one, Vector3 two)
	{

		var result = one;

		result.x *= two.x;
		result.y *= two.y;
		result.z *= two.z;

		return result;

	}

}
