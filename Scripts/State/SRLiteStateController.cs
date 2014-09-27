using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SRLiteStateController<T> : SRMonoBehaviourEx where T : struct
{

	protected SimpleStateMachine<T> StateMachine { get { return _stateMachine; }}

	public bool ShowDebug;

	public T State
	{
		get { return StateMachine.CurrentState; }
		set { StateMachine.SetState(value); }
	}

	protected virtual T DefaultState { get { return default(T); } }

	private SimpleStateMachine<T> _stateMachine;

	protected virtual void OnEnable()
	{
		EnsureStateMachine();
	}

	protected virtual void OnDisable()
	{

	}

	void EnsureStateMachine()
	{

		// Ensure state machine exists. (Can be destroyed by script reload)
		if (_stateMachine != null)
			return;

		_stateMachine = new SimpleStateMachine<T>(DefaultState);
		Configure();

	}

	protected virtual void Update()
	{
		_stateMachine.Update();
	}

	/// <summary>
	/// Override in subclasses to configure state machine
	/// </summary>
	protected virtual void Configure()
	{

	}

	public bool IsInState(T state)
	{
		return EqualityComparer<T>.Default.Equals(_stateMachine.CurrentState,  state);
	}

}
