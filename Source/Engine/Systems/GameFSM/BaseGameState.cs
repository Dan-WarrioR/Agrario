namespace Source.Engine.Systems.GameFSM
{
	public abstract class BaseGameState
	{
		protected GameStateMachine StateMachine { get; private set; }

		public void Initialize(GameStateMachine gameStateMachine)
		{
			StateMachine = gameStateMachine;
		}

		public abstract void Enter();

		public abstract void Update(float deltaTime);

		public abstract void Exit();
	}
}