using Scripts.Framework.Components;
using UnityEngine;

namespace Scripts.Framework.Helpers
{
	public static class SRLineUtil
	{

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
