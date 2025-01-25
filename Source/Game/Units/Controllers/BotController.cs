using SFML.System;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Game.Units.Controllers
{
	public class BotController : BaseController
	{
		private const int MinMovementDelay = 2;
		private const int MaxMovementDelay = 10;

		private Player _target;

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;

		private Vector2f _delta;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			_target = (Player)Target;
		}

		public override void Start()
		{
			base.Start();

			SetRandomDelta();
			_movementDelay = CustomRandom.Range(MinMovementDelay, MaxMovementDelay);
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			_aiMovementTime += deltaTime;

			if (_aiMovementTime > _movementDelay)
			{
				SetRandomDelta();
			}

			var positionDelta = _target.CurrentSpeed * deltaTime * _delta;

			var newPosition = GetClampedPosition(_target.Position + positionDelta);

			_target.SetPosition(newPosition);		
		}

		private void SetRandomDelta()
		{
			_aiMovementTime = 0f;

			float angle = CustomRandom.NextSingle() * MathF.PI * 2;

			_delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}