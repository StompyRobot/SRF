using UnityEngine;

public static class SRGizmos
{

	public static void DrawCircle(Vector3 source, float radius)
	{

		const int segments = 60;


		var radiusStep = 360.0f / segments * Mathf.Deg2Rad;

		var p = 0.0f;

		Vector3 prev = source + Vector3.left*radius;


		for (int i = -1; i <= segments; i++) {

			var point = new Vector3(Mathf.Cos(p) * radius, Mathf.Sin(p) * radius, 0);
			Gizmos.DrawLine(prev, point);

			prev = point;
			p += radiusStep;

		}

	}

}
