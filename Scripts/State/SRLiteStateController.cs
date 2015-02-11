using System.Collections.Generic;

namespace SRF.State
{

	public abstract class SRLiteStateController<T> : SRMonoBehaviourEx where T : struct
	{

		protected SimpleStateMachine<T> StateMachine { get { return _stateMachine; } }

		public bool ShowDebug;

		public T State
		{
			get { return StateMachine.CurrentState; }
			set { StateMachine.SetState(value); }
		}

		protected virtual T DefaultState { get { return default(T); } }

		private SimpleStateMachine<T> _stateMachine;

		protected override void OnEnable()
		{
			base.OnEnable();
			EnsureStateMachine();
		}

		void EnsureStateMachine()
		{

			// Ensure state machine exists. (Can be destroyed by script reload)
			if (_stateMachine != null)
				return;

			_stateMachine = new SimpleStateMachine<T>(DefaultState);
			Configure();

		}

		protected override void Update()
		{
			base.Update();
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
			return EqualityComparer<T>.Default.Equals(_stateMachine.CurrentState, state);
		}

	}

}