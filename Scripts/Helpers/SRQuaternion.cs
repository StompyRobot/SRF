using UnityEngine;
using System.Collections;

public static class SRQuaternion
{

	public static Quaternion LookRotation2D(Vector3 targetDirection)
	{

		Vector2 from = Vector2.up;

		float ang = Vector2.Angle(from, targetDirection);
		Vector3 cross = Vector3.Cross(from, targetDirection);

		if (cross.z > 0)
			ang = 360 - ang;

		ang *= -1f;

		return Quaternion.Euler(0, 0, ang);

	}

}