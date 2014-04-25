using UnityEngine;

namespace Scripts.Framework.Components
{
	public abstract class SRFadeRenderer : SRMonoBehaviour
	{

		[Range(0, 1)]
		public float Alpha = 1.0f;

	}
}
