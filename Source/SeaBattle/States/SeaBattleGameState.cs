using Source.Engine.Systems.GameFSM;
using Source.Engine.Systems.SceneSystem;
using Source.SeaBattle.Scenes;

namespace Source.SeaBattle.States
{
    public class SeaBattleGameState : BaseGameState
    {
        public override void Enter()
        {
            SceneLoader.LoadScene<SeaBattleGameScene>();
        }

        public override void Exit()
        {
            SceneLoader.UnloadCurrentScene();
        }
    }
}