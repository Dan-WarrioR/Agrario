using Source.Engine;
using Source.Engine.Systems;
using Source.Engine.Systems.GameFSM;
using Source.Engine.Systems.SceneSystem;
using Source.Engine.Tools;
using Source.SeaBattle.Scenes;
using Source.SeaBattle.States;

namespace Source.SeaBattle
{
    public class SeaBattleGameRules : BaseGameRules
    {
        private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
        private EventBus _eventBus;

        private GameStateMachine _gameStateMachine;

        public override void Initialize()
        {
            Dependency.Register(this);

            SceneLoader.AddScene<SeaBattleGameScene>();
            SceneLoader.AddScene<MainMenuScene>();

            _gameStateMachine = new();
            _gameStateMachine.SetState<SeaBattleGameState>();

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