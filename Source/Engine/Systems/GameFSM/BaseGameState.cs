namespace Source.Engine.Systems.GameFSM
{
	public abstract class BaseGameState
	{
		protected GameStateMachine StateMachine { get; private set; }

		public void Initialize(GameStateMachine gameStateMachine)
		{
			StateMachine = gameStateMachine;
		}

		public virtual void Enter() { }

		public virtual void Update(float deltaTime) { }

		public virtual void Exit() { }
	}
}