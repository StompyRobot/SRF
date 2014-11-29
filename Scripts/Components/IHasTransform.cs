using UnityEngine;

namespace SRF
{

	public interface IHasTransform
	{

		Transform CachedTransform { get; }

	}

}