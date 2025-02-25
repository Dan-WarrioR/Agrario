using SFML.Graphics;

namespace Source.Engine.Systems.Animation
{
	internal class AnimationStateMachine
	{
		private readonly Dictionary<string, AnimationState> _states = new();
		private readonly Dictionary<AnimationState, List<AnimationTransition>> _transitionMap = new();

		private AnimationState? _currentState;

		public void AddState(AnimationState state)
		{
			_transitionMap.TryAdd(state, new());
			_states.TryAdd(state.Name, state);
		}

		public void AddTransition(AnimationTransition transition)
		{
			if (!_transitionMap.TryGetValue(transition.FromState, out var transitions))
			{
				return;
			}
			
			transitions.Add(transition);
		}

		public void ChangeState(string stateName)
		{
			if (!_states.TryGetValue(stateName, out var newState) || _currentState == newState)
			{
				return;			
			}

			SwitchState(newState);
		}

		public void ChangeState(AnimationState newState)
		{
			if (!_states.ContainsValue(newState) || _currentState == newState)
			{
				return;			
			}

			SwitchState(newState);
		}

		public void Update(float deltaTime)
		{
			if (_transitionMap.TryGetValue(_currentState, out var transitions))
			{
				foreach (var transition in transitions)
				{
					if (transition.CanTransition())
					{
						SwitchState(transition.ToState);

						break;
					}
				}
			}

			_currentState?.Update(deltaTime);
		}

		public Texture? GetCurrentFrame()
		{
			return _currentState?.GetCurrentFrame();
		}

		public void SetFrame(string stateName, List<Texture> frames)
		{
			if (!_states.TryGetValue(stateName, out var state))
			{
				return;
			}

			state.SetFrames(frames);
		}

		private void SwitchState(AnimationState state)
		{
			_currentState?.Exit();
			_currentState = state;
		}
	}
}