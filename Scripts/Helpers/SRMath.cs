using System;
using System.Linq;
using System.Runtime.Remoting.Metadata;
using Holoville.HOTween;
using UnityEngine;
using System.Collections;

public static class SRMath
{

	public static bool IsWithin(float value, float min, float max)
	{

		if (value >= min && value <= max)
			return true;

		return false;
		
	}
	
	public static bool IsWithin(int value, int min, int max)
	{

		if (value >= min && value <= max)
			return true;

		return false;

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

		var t = apDotAb/apLength;
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
	/// Lerp from one value to another, without clamping t to 0-1.
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static float LerpUnclamped(float from, float to, float t)
	{
		return (1.0f - t) * from + t * to;
	}

	/// <summary>
	/// Lerp from one vector to another, without clamping t
	/// </summary>
	/// <param name="from"></param>
	/// <param name="to"></param>
	/// <param name="t"></param>
	/// <returns></returns>
	public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float t)
	{
		return new Vector3(
			LerpUnclamped(from.x, to.x, t),
			LerpUnclamped(from.y, to.y, t),
			LerpUnclamped(from.z, to.z, t)
			);
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

		var b = 2.0f*Vector3.Dot(lineDir, v);
		var c = Vector3.Dot(v, v) - sphereRadius*sphereRadius;

		var d = b*b - 4.0f*c;

		if (d < 0.0f) {
			return float.PositiveInfinity;
		}

		var s = Mathf.Sqrt(d);
		var t1 = (-b - s)*0.5f;
		var t2 = (-b + s)*0.5f;

		if (t1 > 0.0f)
			return t1;

		if (t2 > 0.0f)
			return t2;

		return float.PositiveInfinity;

	}

	/// <summary>
	/// Value from 0.0f-1.0f, 0 when facing fully away and 1.0f when facing fully towards
	/// </summary>
	public static float FacingNormalized(Vector3 dir1, Vector3 dir2)
	{

		dir1.Normalize();
		dir2.Normalize();

		return Mathf.InverseLerp(-1, 1, Vector3.Dot(dir1, dir2));

	}

	private static TweenVar _smoothClampTween = new TweenVar(0, 1.0f, 1.0f, EaseType.EaseOutExpo);

	/// <summary>
	/// Smoothly clamp value between 0 and max, smoothing between min and max
	/// </summary>
	/// <param name="value"></param>
	/// <param name="min"></param>
	/// <param name="max"></param>
	/// <param name="easeType"></param>
	/// <returns></returns>
	public static float SmoothClamp(float value, float min, float max, float scrollMax, EaseType easeType = EaseType.EaseOutExpo)
	{

		if (value < min)
			return value;

		var p = Mathf.Clamp01((value - min)/(scrollMax - min));

		Debug.Log(p);

		_smoothClampTween.easeType = easeType;

		return Mathf.Clamp(min + Mathf.Lerp(value - min, max, _smoothClampTween.Update(p)), 0, max);

	}

	/// <summary>
	/// Clamp value between min and max, but smoothly.
	/// </summary>
	public static float SpringClamp(float value, float min, float max, float springStrength, float dt)
	{
		throw new NotImplementedException();
		/*
		if (value < min) {
			return value + (min - value)*NGUIMath.SpringLerp(springStrength, dt);
		}

		if (value > max) {
			return value + (max - value)*NGUIMath.SpringLerp(springStrength, dt);
		}

		return value;
		*/
	}

	/// <summary>
	/// Reduces a given angle to a value between 180 and -180.
	/// </summary>
	/// <param name="angle">The angle to reduce, in radians.</param>
	/// <returns>The new angle, in radians.</returns>
	/// https://github.com/mono/MonoGame/blob/develop/MonoGame.Framework/MathHelper.cs
	public static float WrapAngle(float angle)
	{
		if (angle <= -180f) {
			angle += 360f;
		} else {
			if (angle > 180f) {
				angle -= 360f;
			}
		}
		return angle;
	}

	/// <summary>
	/// Return the angle closest to 'to'
	/// </summary>
	/// <param name="to"></param>
	/// <param name="angle1"></param>
	/// <param name="angle2"></param>
	/// <returns></returns>
	public static float NearestAngle(float to, float angle1, float angle2)
	{

		if (Mathf.Abs(Mathf.DeltaAngle(to, angle1)) > Mathf.Abs(Mathf.DeltaAngle(to, angle2))) {

			return angle2;

		} else {

			return angle1;

		}

	}

	public static int Wrap(int max, int value)
	{

		if (max < 0)
			throw new ArgumentOutOfRangeException("max", "max must be greater than 0");

		while (value < 0)
			value += max;

		while (value >= max)
			value -= max;

		return value;

	}

	public static float Wrap(float max, float value)
	{

		while (value < 0)
			value += max;

		while (value > max)
			value -= max;

		return value;

	}

	public static float Average(float v1, float v2)
	{
		return (v1 + v2)*0.5f;
	}

	/// <summary>
	/// Return an angle in range -180, 180 based on direction vector
	/// </summary>
	/// <param name="direction"></param>
	/// <returns></returns>
	public static float Angle(Vector2 direction)
	{

		var angle = Vector3.Angle(Vector3.up, direction);

		if (Vector3.Cross(direction, Vector3.up).z > 0f)
			angle *= -1;

		return angle;

	}

}
