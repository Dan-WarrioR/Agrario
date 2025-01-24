using SFML.System;
using Source.Engine.GameObjects;
using Random = Source.Engine.Tools.Random;

namespace Source.Game.Units.Components
{
	public class AiMovementComponent : BaseComponent
	{
		private const int MinMovementDelay = 2;
		private const int MaxMovementDelay = 10;

		private Vector2f _delta;

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;

		private Player _player;

		public override void Start()
		{
			_player = (Player)Owner;

			SetDelta();
			_aiMovementTime = 0f;

			_movementDelay = Random.Range(MinMovementDelay, MaxMovementDelay);
		}

		public override void Update(float deltaTime)
		{
			_aiMovementTime += deltaTime;

			if (_aiMovementTime > _movementDelay)
			{
				SetDelta();
				_aiMovementTime = 0f;
			}	

			Vector2f positionDelta = _player.CurrentSpeed * deltaTime * _delta;

			_player.SetPosition(_player.Position + positionDelta);

		}

		private void SetDelta()
		{
			float angle = Random.NextSingle() * MathF.PI * 2;

			_delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}