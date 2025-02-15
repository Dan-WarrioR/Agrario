using SFML.Graphics;

namespace Source.Engine.Systems.Animation
{
	public class AnimationStateMachine
	{
		private readonly Dictionary<string, AnimationState> _states = new();
		private readonly List<AnimationTransition> _transitions = new();

		private AnimationState? _currentState;

		public void AddState(AnimationState state)
		{
			_states.TryAdd(state.Name, state);
		}

		public void AddTransition(AnimationTransition transition)
		{
			_transitions.Add(transition);
		}

		public void SetState(string stateName)
		{
			if (!_states.TryGetValue(stateName, out var newState) || _currentState == newState)
			{
				return;			
			}

			_currentState?.Reset();
			_currentState = newState;
		}

		public void Update(float deltaTime)
		{
			foreach (var transition in _transitions)
			{
				if (transition.FromState == _currentState?.Name && transition.CanTransition())
				{
					SetState(transition.ToState);

					break;
				}
			}

			_currentState?.Update(deltaTime);
		}

		public Texture? GetCurrentFrame()
		{
			return _currentState?.GetCurrentFrame();
		}
	}
}