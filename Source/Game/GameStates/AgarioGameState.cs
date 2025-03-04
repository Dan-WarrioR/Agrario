using Source.Engine.Systems.GameFSM;
using Source.Engine.Systems.SceneSystem;
using Source.Game.Scenes;

namespace Source.Game.GameStates
{
    public class AgarioGameState : BaseGameState
	{
		public override void Enter()
		{
			SceneLoader.LoadScene<GameBaseScene>();
		}

		public override void Exit()
		{
			SceneLoader.UnloadCurrentScene();
		}
	}
}