namespace Source.Engine.Systems.GameFSM
{
    public class GameStateMachine
    {
        private BaseGameState _currentState;

        public void SetState(BaseGameState newState)
        {
            _currentState?.Exit();
            _currentState = newState;
            _currentState.Enter();
        }

        public void Update(float deltaTime)
        {
            _currentState?.Update(deltaTime);
        }
    }
}