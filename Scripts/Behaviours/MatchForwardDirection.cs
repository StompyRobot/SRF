using UnityEngine;
using System.Collections;
using SRF.Internal;

namespace SRF.Behaviours
{

	/// <summary>
	/// Rotate transform of attached object so the forward direction vectors of both objects match
	/// </summary>
	[AddComponentMenu(ComponentMenuPaths.MatchForwardDirection)]
	public class MatchForwardDirection : SRMonoBehaviour
	{

		public Transform Target;

		protected virtual void Start()
		{

			AssertNotNull(Target, "Target");

		}

		private void Update()
		{
			CachedTransform.forward = Target.forward;
		}

	}

}