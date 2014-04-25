using UnityEngine;
using System.Collections;

public static class EffectUtils
{

	public static void AttachEffect(GameObject effect, Transform target)
	{

		var match = effect.GetComponentOrAdd<MatchTransform>();
		match.Target = target;

	}

	public static void AttachEffectWithCameraOffset(GameObject effect, Transform target, float offset = 30)
	{

		var match = effect.GetComponentOrAdd<MatchTransform>();

		match.Target = target;

		if (Camera.main != null) {
			var cameraDirection = Camera.main.transform.position - target.position;
			match.PositionOffset = cameraDirection.normalized*offset;
		}

	}

}
