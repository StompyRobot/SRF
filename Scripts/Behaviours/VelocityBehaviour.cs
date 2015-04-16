using SRF.Internal;
using UnityEngine;

namespace SRF.Behaviours
{

	/// <summary>
	/// Simple behaviour, moves the attached transform by Velocity*dt every update.
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.Velocity)]
	public class VelocityBehaviour : SRMonoBehaviour
	{

		public Vector3 Velocity;

		void Update()
		{

			CachedTransform.position += Velocity * Time.deltaTime;

		}

	}

}