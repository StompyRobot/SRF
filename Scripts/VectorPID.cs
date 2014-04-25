using System;
using UnityEngine;
using System.Collections;

/// <summary>
/// A very simple PID controller component class.
/// </summary>
[Serializable]
public class VectorPID
{

	[SerializeField]
	public float Kp = 1;

	[SerializeField]
	public float Ki = 0;

	[SerializeField]
	public float Kd = 0.1f;

	private Vector3 _p, _i, _d;
	private Vector3 _prevError;

	public Vector3 GetOutput(Vector3 currentError, float deltaTime)
	{
		_p = currentError;
		_i += _p * deltaTime;
		_d = (_p - _prevError) / deltaTime;
		_prevError = currentError;

		return _p * Kp + _i * Ki + _d * Kd;
	}
}
