namespace Source.Engine.Systems.Animation
{
	public class AnimationTransition
	{
		public AnimationState FromState { get; }
		public AnimationState ToState { get; }

		private readonly Func<bool> _condition;

		public AnimationTransition(AnimationState fromState, AnimationState toState, Func<bool> condition)
		{
			FromState = fromState;
			ToState = toState;
			_condition = condition;
		}

		public bool CanTransition()
		{
			return _condition.Invoke();
		}
	}
}