using Source.Engine.Systems.GameFSM;
using Source.Engine.Tools;
using Source.Engine.Systems.SceneSystem;
using Source.Engine.Systems;
using Source.Game.Scenes;

namespace Source.Game.GameStates
{
    public class MainMenuState : BaseGameState
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

		public override void Enter()
		{
			SceneLoader.LoadScene<MainMenuBaseScene>();
			EventBus.Register("OnGameStart", OnStartGame);
		}	

		public override void Exit()
		{
			EventBus.Unregister("OnGameStart", OnStartGame);

			SceneLoader.UnloadCurrentScene();
		}

		private void OnStartGame()
		{
			StateMachine.SetState<AgarioGameState>();
		}
	}
}