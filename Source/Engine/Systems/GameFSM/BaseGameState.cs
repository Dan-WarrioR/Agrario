namespace Source.Engine.Systems.GameFSM
{
	public abstract class BaseGameState
	{
		public abstract void Enter();

		public abstract void Update(float deltaTime);

		public abstract void Exit();
	}
}