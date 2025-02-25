using SFML.Graphics;
using Source.Engine.Systems.Animation;
using Source.Engine.Systems.Animation.Conditions;

namespace Source.Engine.GameObjects.Components
{
	public class Animator : BaseComponent
	{
		private ShapeObject _target;
		private AnimationStateMachine _stateMachine = new();
		private readonly Dictionary<string, BaseAnimationParameter> _parameters = new();

		public override void Start()
		{
			_target = (ShapeObject)Owner;
		}

		public void Setup(AnimationGraphBuilder builder)
		{
			var data = builder.Build();
			
			foreach (var states in data.States)
			{
				_stateMachine.AddState(states);
			}
			
			foreach (var transition in data.Transitions)
			{
				_stateMachine.AddTransition(transition);
			}

			foreach (var parameter in data.Parameters)
			{
				_parameters.TryAdd(parameter.Key, parameter.Value);
			}
			
			_stateMachine.ChangeState(data.InitialState);
		}

		public override void Update(float deltaTime)
		{
			_stateMachine.Update(deltaTime);
			var frame = _stateMachine.GetCurrentFrame();
			_target.SetTexture(frame);
		}

		public bool GetBool(string name)
		{
			if (!_parameters.TryGetValue(name, out var parameter))
			{
				return false;
			}

			return parameter.GetValue<bool>();
		}

		public void SetBool(string name, bool value)
		{
			if (_parameters.TryGetValue(name, out var parameter))
			{
				parameter.SetValue(value);
			}
		}

		public void SetFrames(string stateName, List<Texture> frames)
		{
			_stateMachine.SetFrame(stateName, frames);
		}
	}
}