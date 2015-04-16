using UnityEngine;

namespace SRF.Components
{

	/// <summary>
	/// Base class for all Fade Renderers.
	/// Setting the Alpha on this component will handle fading an object, with no details of the implementation
	/// leaking out.
	/// </summary>
	public abstract class SRFadeRenderer : SRMonoBehaviour
	{

		[Range(0, 1)]
		public float Alpha = 1.0f;

	}

}
