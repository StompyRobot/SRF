using UnityEngine;
using System.Collections;

/// <summary>
/// Rotate transform of attached object so the forward direction vectors of both objects match
/// </summary>
public class MatchForwardDirection : SRMonoBehaviour
{

	public Transform Target;

	protected virtual void Start()
	{

		AssertNotNull(Target, "Target");

	}

	void Update()
	{
		CachedTransform.forward = Target.forward;
	}

}
