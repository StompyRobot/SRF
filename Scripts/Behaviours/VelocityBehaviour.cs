using UnityEngine;
using System.Collections;

public class VelocityBehaviour : SRMonoBehaviour
{

	public Vector3 Velocity;

	void Update()
	{

		CachedTransform.position += Velocity*Time.deltaTime;

	}

}
