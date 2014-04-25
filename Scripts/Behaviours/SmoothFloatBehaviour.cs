using UnityEngine;
using System.Collections;

public class SmoothFloatBehaviour : SRMonoBehaviour
{

	public Vector2 FloatLimits;

	public float FloatSpeed = 3;
	public float Smooth = 3.5f;

	public float TimeOffset;

	private Vector3 _targetPosition;

	private void Update()
	{

		_targetPosition = Vector3.zero;
		_targetPosition.x += FloatLimits.x * Mathf.Sin(Time.realtimeSinceStartup + TimeOffset)*FloatSpeed;
		_targetPosition.y += FloatLimits.y * Mathf.Cos(Time.realtimeSinceStartup + TimeOffset)*FloatSpeed;

		CachedTransform.localPosition = Vector3.Lerp(CachedTransform.localPosition, _targetPosition,
		                                             Smooth*RealTime.deltaTime);

	}

}
