using System.Linq;
using UnityEngine;
using System.Collections;
using Smooth.Slinq

public static partial class SRMath
{

	public static bool IsWithin(float value, float min, float max)
	{
		return value >= min && value <= max;
	}

	public static bool IsWithin(int value, int min, int max)
	{
		return value >= min && value <= max;
	}

	/// <summary>
	/// Returns the closest point on line ab to point p
	/// </summary>
	/// <param name="a">Line point 1</param>
	/// <param name="b">Line point 2</param>
	/// <param name="p">Test point</param>
	/// <source>http://stackoverflow.com/a/3122532</source>
	/// <returns></returns>
	public static Vector3 ClosestPointOnLine(Vector3 a, Vector3 b, Vector3 p)
	{

		var ap = p - a;
		var ab = b - a;

		var apLength = ab.sqrMagnitude;

		var apDotAb = Vector3.Dot(ap, ab);

		var t = apDotAb / apLength;
		t = Mathf.Clamp01(t);

		return a + ab * t;

	}

	/// <summary>
	/// Return closest point on box to point p
	/// </summary>
	/// <param name="b4"></param>
	/// <param name="p"></param>
	/// <param name="b1"></param>
	/// <param name="b2"></param>
	/// <param name="b3"></param>
	/// <returns></returns>
	public static Vector3 ClosestPointOnBox(Vector3 a, Vector3 b, Vector3 c, Vector3 d, Vector3 p)
	{

		var abPoint = ClosestPointOnLine(a, b, p);
		var bcPoint = ClosestPointOnLine(b, c, p);
		var cdPoint = ClosestPointOnLine(c, d, p);
		var daPoint = ClosestPointOnLine(d, a, p);

		return NearestPoint(p, abPoint, bcPoint, cdPoint, daPoint);

	}

	/// <summary>
	/// Compute the nearest pair of points to p. Much more inefficient than the specialised versions below. BROKEN ON iOS
	/// </summary>
	/// <param name="p">Point to test from</param>
	/// <param name="o1"></param>
	/// <param name="o2"></param>
	/// <param name="points"></param>
	public static void NearestPair(Vector3 p, out Vector3 o1, out Vector3 o2, params Vector3[] points)
	{

		System.Diagnostics.Debug.Assert(points.Length > 1);

		// Early out if only two points
		if (points.Length == 2) {
			o1 = points[0];
			o2 = points[1];
			return;
		}

		var l = points.OrderBy(q => Vector3.SqrMagnitude(p - q));
		o1 = l.ElementAt(0);
		o2 = l.ElementAt(1);

	}

	/// <summary>
	/// Same as params version above, but optimised for 4 points
	/// </summary>
	/// <param name="p"></param>
	/// <param name="o1"></param>
	/// <param name="o2"></param>
	/// <param name="v1"></param>
	/// <param name="v2"></param>
	/// <param name="v3"></param>
	/// <param name="v4"></param>
	public static void NearestPair(Vector3 p, out Vector3 o1, out Vector3 o2, Vector3 v1, Vector3 v2, Vector3 v3,
								   Vector3 v4)
	{

		NearestPair(p, out o1, out o2, v1, v2, v3);
		NearestPair(p, out o1, out o2, o1, o2, v4);

	}

	/// <summary>
	/// Calculate nearest pair of points to p
	/// </summary>
	/// <param name="p"></param>
	/// <param name="o1"></param>
	/// <param name="o2"></param>
	/// <param name="v1"></param>
	/// <param name="v2"></param>
	/// <param name="v3"></param>
	public static void NearestPair(Vector3 p, out Vector3 o1, out Vector3 o2, Vector3 v1, Vector3 v2, Vector3 v3)
	{

		var dist1 = Vector3.SqrMagnitude(v1 - p);
		var dist2 = Vector3.SqrMagnitude(v2 - p);
		var dist3 = Vector3.SqrMagnitude(v3 - p);

		if (dist1 < dist2) {

			o1 = v1;
			o2 = dist3 < dist2 ? v3 : v2;

		} else {

			o1 = v2;
			o2 = dist3 < dist1 ? v3 : v1;

		}
	}

	/// <summary>
	/// Return the point nearest to p
	/// </summary>
	/// <param name="p"></param>
	/// <param name="v1"></param>
	/// <param name="v2"></param>
	/// <returns></returns>
	public static Vector3 NearestPoint(Vector3 p, Vector3 v1, Vector3 v2)
	{

		var d1 = (v1 - p).sqrMagnitude;
		var d2 = (v2 - p).sqrMagnitude;

		return d1 > d2 ? v2 : v1;
	}

	public static Vector3 NearestPoint(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3)
	{
		return NearestPoint(p, NearestPoint(p, v1, v2), v3);
	}

	public static Vector3 NearestPoint(Vector3 p, Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4)
	{
		return NearestPoint(p, NearestPoint(p, NearestPoint(p, v1, v2), v3), v4);
	}

	/// <summary>
	/// Return distance along line (lineOrigin, lineDir) until collision with sphere (sphereOrigin, sphereRadius).
	/// </summary>
	/// <param name="lineOrigin"></param>
	/// <param name="lineDir"></param>
	/// <param name="sphereOrigin"></param>
	/// <param name="sphereRadius"></param>
	/// <returns>Distance to hit if successful, or float.PositiveInfinity if no hit.</returns>
	public static float SphereIntersect(Vector3 lineOrigin, Vector3 lineDir, Vector3 sphereOrigin, float sphereRadius)
	{

		var v = lineOrigin - sphereOrigin;

		var b = 2.0f * Vector3.Dot(lineDir, v);
		var c = Vector3.Dot(v, v) - sphereRadius * sphereRadius;

		var d = b * b - 4.0f * c;

		if (d < 0.0f) {
			return float.PositiveInfinity;
		}

		var s = Mathf.Sqrt(d);
		var t1 = (-b - s) * 0.5f;
		var t2 = (-b + s) * 0.5f;

		if (t1 > 0.0f)
			return t1;

		if (t2 > 0.0f)
			return t2;

		return float.PositiveInfinity;

	}


}