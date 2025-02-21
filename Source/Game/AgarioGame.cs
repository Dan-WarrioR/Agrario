using Source.Engine;
using Source.Engine.Systems.GameFSM;
using Source.Engine.Tools;
using Source.Game.GameStates;

namespace Source.Game
{
	public class AgarioGame : BaseGame
	{
		private GameStateMachine _gameStateMachine;

		public override void Initialize()
		{
			Dependency.Register(this);

			_gameStateMachine = new();

			_gameStateMachine.SetState<MainMenuState>();
		}

		public override void OnUpdate(float deltaTime)
		{
			_gameStateMachine.Update(deltaTime);
		}
	}
}