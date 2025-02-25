using Source.Engine;
using Source.Engine.Systems;
using Source.Engine.Systems.GameFSM;
using Source.Engine.Systems.SceneSystem;
using Source.Engine.Tools;
using Source.Game.GameStates;
using Source.Game.Scenes;

namespace Source.Game
{
	public class AgarioGame : BaseGame
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

		private GameStateMachine _gameStateMachine;

		public override void Initialize()
		{
			Dependency.Register(this);

			SceneLoader.AddScene<MainMenuScene>();
			SceneLoader.AddScene<GameScene>();

			_gameStateMachine = new();
			_gameStateMachine.SetState<MainMenuState>();

			EventBus.Register("OnStopGame", StopGame);
		}

		public override void OnDestroy()
		{
			EventBus.Unregister("OnStopGame", StopGame);
		}

		public override void OnUpdate(float deltaTime)
		{
			_gameStateMachine.Update(deltaTime);
		}
	}
}