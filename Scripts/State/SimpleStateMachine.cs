using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

/// <summary>
/// Simple state machine providing OnEntry/OnExit events 
/// </summary>
/// <typeparam name="T">Enum type representing states</typeparam>
public class SimpleStateMachine<T> where T : struct 
{

	public class StateConfiguration
	{

		public StateConfiguration OnEntry(Action action)
		{
			Entry = action;
			return this;
		}
	
		public StateConfiguration OnExit(Action action)
		{
			Exit = action;
			return this;
		}
	
		public StateConfiguration OnUpdate(Action action)
		{
			Update = action;
			return this;
		}

		public Action Entry;
		public Action Exit;
		public Action Update;

	}

	private readonly Dictionary<T, StateConfiguration> _internalStore = new Dictionary<T, StateConfiguration>();

	private T _currentState;

	// Current state cached update function (reduce dictionary lookups)
	private Action _currentUpdate; 

	public T CurrentState { get { return _currentState; } set { SetState(value); } }

	/// <summary>
	/// Configure possible state value
	/// </summary>
	/// <param name="state">State value to configure</param>
	public StateConfiguration Configure(T state)
	{

		StateConfiguration store;
		
		// Try and fetch existing configuration if it exists
		if(!_internalStore.TryGetValue(state, out store))
		{
			store = _internalStore[state] = new StateConfiguration();
		}

		return store;

	}

	public SimpleStateMachine(T initialState)
	{
		_currentState = initialState;
	}

	/// <summary>
	/// Set the current state, firing any Exit/Entry events.
	/// </summary>
	/// <param name="state">New state</param>
	/// <param name="preventReentry">Prevent events firing if no state change occurs</param>
	public void SetState(T state, bool preventReentry = true)
	{
		
		// Do nothing if no change
		if (preventReentry && EqualityComparer<T>.Default.Equals(state, _currentState))
			return;

		// Store origin state for later
		var origin = CurrentState;

		// Change current state now, in case events cause further state changes
		_currentState = state;

		// Set to null in case new state has no update function set
		_currentUpdate = null;

		StateConfiguration store;

		// Call origin state OnExit
		if (_internalStore.TryGetValue(origin, out store) && store.Exit != null) {
			store.Exit();
		}
	
		if (_internalStore.TryGetValue(_currentState, out store)) {

			// Call destination state OnEntry
			if (store.Entry != null)
				store.Entry();

			// Cache update function
			_currentUpdate = store.Update;

		}

	}

	/// <summary>
	/// Call update function of current state
	/// </summary>
	public void Update()
	{

		if (_currentUpdate != null)
			_currentUpdate();

	}
	
}
