using System.Collections.Generic;
using Scripts.Framework.Components;
using UnityEngine;

namespace Scripts.Framework.Helpers
{
	public static class SRLineUtil
	{

		public static void SimplifyLine(IList<Vector3> src, IList<Vector3> dest, float minDistSqr, float dotThreshold = 0.99f)
		{

			dest.Clear();

			if (src.Count == 0)
				return;

			dest.Add(src[0]);

			if (src.Count == 1) 
				return;

			if (src.Count == 2) {
				dest.Add(src[1]);
				return;
			}

			var p = src[0];
			var dir = p.DirectionTo(src[1]);

			for (var i = 1; i < src.Count-1; i++) {

				var newPoint = src[i];

				var dist = Vector3Extensions.DistanceSquared(p, newPoint);

				if(dist < minDistSqr)
					continue;

				var newDir = p.DirectionTo(newPoint);

				if (i > 2 && Vector3.Dot(dir, newDir) > dotThreshold)
					continue;

				dir = newDir;

				dest.Add(newPoint);
				p = newPoint;

			}

			dest.Add(src[src.Count - 1]);

		}

		public static void CreateCircle(LineRenderer line, float radius, int segments)
		{

			var radiusStep = 360.0f / segments * Mathf.Deg2Rad;

			var p = 0.0f;

			for (int i = 0; i <= segments; i++) {

				line.SetPosition(i, new Vector3(Mathf.Cos(p)*radius, 0, Mathf.Sin(p)*radius));

				p += radiusStep;

			}

		}
	
		public static void CreateCircle(ExLineRenderer line, float radius, int segments)
		{

			line.SetVertexCount(segments+1);

			var radiusStep = 360.0f / segments * Mathf.Deg2Rad;

			var p = 0.0f;

			for (int i = 0; i <= segments; i++) {

				line.SetPosition(i, new Vector3(Mathf.Cos(p)*radius, 0, Mathf.Sin(p)*radius));

				p += radiusStep;

			}

		}	
		
		public static void CreateCircle(TangentLineRenderer line, float radius, int segments)
		{

			var radiusStep = 360.0f / segments * Mathf.Deg2Rad;

			var p = 0.0f;

			for (int i = 0; i <= segments; i++) {

				line.SetPosition(i, new Vector3(Mathf.Cos(p)*radius, 0, Mathf.Sin(p)*radius));

				p += radiusStep;

			}

		}

	}
}
