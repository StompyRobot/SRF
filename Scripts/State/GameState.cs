using System;


namespace SRF.State
{

	public abstract class GameState<TStates, TTriggers> : SRMonoBehaviour
	{

		public abstract TStates State { get; }

		public class StateMachine : Stateless.StateMachine<TStates, TTriggers>
		{

			public StateMachine(Func<TStates> stateAccessor, Action<TStates> stateMutator) : base(stateAccessor, stateMutator) { }

			public StateMachine(TStates initialState) : base(initialState) { }

		}

		protected StateMachine ActiveStateMachine { get; private set; }

		protected virtual void Awake()
		{
			// Default to disabled. We get enabled in OnEntry later
			enabled = false;
		}

		public void Configure(StateMachine machine)
		{

			ActiveStateMachine = machine;
			var config = machine.Configure(State);
			Setup(config);

		}

		protected virtual void Update()
		{
		}

		protected virtual void Setup(StateMachine.StateConfiguration configuration)
		{
			configuration.OnEntry(OnEntry).OnExit(OnExit);
		}

		protected virtual void OnEntry(StateMachine.Transition transition)
		{
			enabled = true;
		}

		protected virtual void OnExit(StateMachine.Transition transition)
		{
			enabled = false;
		}

	}

}