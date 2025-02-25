namespace Source.Engine.Systems.GameFSM
{
    public class GameStateMachine
    {
        private BaseGameState _currentState;

        public void SetState<T>() where T : BaseGameState, new()
        {
            var newState = new T();
			newState.Initialize(this);

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