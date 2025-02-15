using Source.Engine.Systems.Animation;

namespace Source.Engine.GameObjects.Components
{
	public class Animator : BaseComponent
	{
		private ShapeObject _target;
		private AnimationStateMachine _stateMachine = new();

		public override void Start()
		{
			_target = (ShapeObject)Owner;
		}

		public void AddAnimation(AnimationState state)
		{
			_stateMachine.AddState(state);
		}

		public void AddTransition(AnimationTransition transition)
		{
			_stateMachine.AddTransition(transition);
		}

		public void Play(string animationName)
		{
			_stateMachine.SetState(animationName);
		}

		public override void Update(float deltaTime)
		{
			_stateMachine.Update(deltaTime);
			var frame = _stateMachine.GetCurrentFrame();
			_target.SetTexture(frame);
		}
	}
}