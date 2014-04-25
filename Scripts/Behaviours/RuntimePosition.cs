using UnityEngine;
using System.Collections;

/// <summary>
/// Set localPosition as soon as this object is created. (Useful for setting up UI workspaces in editor)
/// </summary>
public class RuntimePosition : MonoBehaviour
{

	public Vector3 RunPosition;

	public bool UseLocal = true;

	void Awake()
	{

		if (UseLocal)
			transform.localPosition = RunPosition;
		else
			transform.position = RunPosition;

		Destroy(this);

	}

}
