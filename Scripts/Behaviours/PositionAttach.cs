using UnityEngine;
using System.Collections;

/// <summary>
/// Attach GameObject to a transform with position only (scale and rotation remain unchanged)
/// </summary>
public class PositionAttach : SRMonoBehaviour
{

	public Transform Attach;
	public Vector3 Offset;

	void Update()
	{
		UpdatePosition();
	}

	void LateUpdate()
	{
		UpdatePosition();
	}

	void UpdatePosition()
	{

		if (Attach == null)
			return;

		CachedTransform.position = Attach.position + Offset;

	}

}
