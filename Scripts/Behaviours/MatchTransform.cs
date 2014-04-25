using UnityEngine;
using System.Collections;

/// <summary>
/// Match the target transform until it is destroyed.
/// </summary>
[ExecuteInEditMode]
public class MatchTransform : SRMonoBehaviour
{

	public Transform Target;

	public Vector3 PositionOffset;

	public bool ExecuteInEditMode = false;

	void LateUpdate()
	{

		if (Target == null)
			return;

		if (Application.isEditor && !Application.isPlaying && !ExecuteInEditMode)
			return;

		CachedTransform.position = Target.position + PositionOffset;
		CachedTransform.rotation = Target.transform.rotation;

	}

}
