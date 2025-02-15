namespace Source.Engine.Systems.Animation
{
	public class AnimationTransition
	{
		public string FromState { get; }
		public string ToState { get; }

		private readonly Func<bool> _condition;

		public AnimationTransition(string fromState, string toState, Func<bool> condition)
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