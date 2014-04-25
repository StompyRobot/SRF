public interface IHasStateMachine<TState, TTrigger>
{

	bool IsInState(TState state);
	void FireTrigger(TTrigger trigger);

}
