using System;
using UnityEngine;

/// <summary>
/// A very simple PID controller component class.
/// </summary>
[Serializable]
public class PID
{

	[SerializeField]
	public float Kp = 1;

	[SerializeField]
	public float Ki = 0;

	[SerializeField]
	public float Kd = 0.1f;

	private float _p, _i, _d;
	private float _prevError;

	public float GetOutput(float currentError, float deltaTime)
	{
		_p = currentError;
		_i += _p * deltaTime;
		_d = (_p - _prevError) / deltaTime;
		_prevError = currentError;
		
		return _p*Kp + _i*Ki + _d*Kd;
	}

}
