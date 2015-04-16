using UnityEngine;

namespace Scripts.Framework.Helpers
{
	public static class SRRandom
	{

		/// <summary>
		/// Return a random position on the game plane (x,z) within magnitude range
		/// </summary>
		/// <param name="minMag"></param>
		/// <param name="maxMag"></param>
		/// <returns></returns>
		public static Vector3 RandomPosition(float minMag, float maxMag)
		{

			var mag = Random.Range(minMag, maxMag);

			return mag*Random.onUnitSphere.Flatten();

		}

		public static Vector2 RandomPosition2D(float minMag, float maxMag)
		{

			var mag = Random.Range(minMag, maxMag);

			return mag*Random.insideUnitCircle.normalized;

		}

	}
}
