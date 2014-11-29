using System;
using System.Linq;
using UnityEngine;

namespace SRF.State
{

	public abstract class SRStateController<TState, TTrigger> : SRMonoBehaviour, IHasStateMachine<TState, TTrigger>
	{

		protected class InternalStateMachine : Stateless.StateMachine<TState, TTrigger>
		{

			public InternalStateMachine(Func<TState> stateAccessor, Action<TState> stateMutator)
				: base(stateAccessor, stateMutator) {}

			public InternalStateMachine(TState initialState) : base(initialState) {}

		}

		public delegate void StateChangedHandler(object sender, TState oldState, TState newState);

		private SRList<StateChangedHandler> _eventListeners;

		// Have to provide custom implementation to fix registering on iOS

		public event StateChangedHandler StateChanged
		{

			add
			{
				if (_eventListeners == null)
					_eventListeners = new SRList<StateChangedHandler>(1);
				_eventListeners.Add(value);
			}

			remove
			{
				if (_eventListeners == null)
					return;
				_eventListeners.Remove(value);
			}

		}

		public bool ShowDebug;

		public TState State
		{
			get { return _state; }
		}

		protected InternalStateMachine StateMachine
		{
			get { return _stateMachine; }
		}

		protected float StateTime
		{
			get { return Time.realtimeSinceStartup - _transTime; }
		}

		protected virtual TState DefaultState
		{
			get { return default(TState); }
		}

		private TState _state;
		private TState _prevState;
		private TTrigger _prevTrigger;

		private InternalStateMachine _stateMachine;
		private float _transTime;

		protected virtual void Awake()
		{
			EnsureStateMachine();
		}

		protected virtual void OnEnable()
		{
			EnsureStateMachine();
		}

		protected virtual void OnDisable() {}

		private void EnsureStateMachine()
		{

			// Ensure state machine exists. (Can be destroyed by script reload)
			if (_stateMachine != null)
				return;

			_state = DefaultState;

			_stateMachine = new InternalStateMachine(() => _state,
				state => _state = state);

			_stateMachine.OnTransitioned(OnStateMachineTransitioned);

			Configure();
		}

		private void OnStateMachineTransitioned(InternalStateMachine.Transition transition)
		{

			_transTime = Time.realtimeSinceStartup;
			_prevState = transition.Source;
			_prevTrigger = transition.Trigger;

			if (_eventListeners != null && _eventListeners.Count > 0) {
				for (var i = 0; i < _eventListeners.Count; ++i) {
					_eventListeners[i](this, transition.Source, transition.Destination);
				}
			}

		}


		/// <summary>
		/// Override in subclasses to configure state machine
		/// </summary>
		protected virtual void Configure() {}

		public bool IsInState(TState state)
		{
			EnsureStateMachine();
			return _stateMachine.IsInState(state);
		}

		public void FireTrigger(TTrigger trigger)
		{
			EnsureStateMachine();
			_stateMachine.Fire(trigger);
		}

#if UNITY_EDITOR

		private static string _nameCache;

		protected virtual void OnGUI()
		{

			if (!ShowDebug)
				return;

			if (_nameCache == null)
				_nameCache = GetType().Name;

			GUILayout.Label("{0} ({1})".Fmt(name, _nameCache));
			GUILayout.Label("State: {0} ({1:0.00}s, Last Transition: {2} -({3})> {0})".Fmt(State, StateTime, _prevState,
				_prevTrigger));
			GUILayout.Label(
				"Triggers: {0}".Fmt(string.Join(", ", _stateMachine.PermittedTriggers.Select(p => p.ToString()).ToArray())));
			GUILayout.Label("StateChanged listener count: {0}".Fmt(_eventListeners != null ? _eventListeners.Count : 0));

		}

#endif

	}

}