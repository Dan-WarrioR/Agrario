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

		public void Setup(AnimationData data)
		{
			foreach (var states in data.GetStates())
			{
				_stateMachine.AddState(states);
			}
			
			foreach (var transition in data.GetTransitions())
			{
				_stateMachine.AddTransition(transition);
			}
			
			_stateMachine.ChangeState(data.InitialState);
		}

		public override void Update(float deltaTime)
		{
			_stateMachine.Update(deltaTime);
			var frame = _stateMachine.GetCurrentFrame();
			_target.SetTexture(frame);
		}
	}
}