using UnityEngine;
using System.Collections;

public class LookAt : SRMonoBehaviour
{

	public Transform Target;

	void LateUpdate()
	{

		if(Target != null)
			CachedTransform.LookAt(Target, Vector3.up);

	}
}
