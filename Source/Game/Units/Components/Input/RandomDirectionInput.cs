using SFML.System;

namespace Source.Game.Units.Components.Input
{
	public class RandomDirectionInput : IInputComponent
	{
		private const int MinMovementDelay = 2;
		private const int MaxMovementDelay = 10;

		public Vector2f Delta { get; private set; }

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;

		public void UpdateInput()
		{
			if (_aiMovementTime < _movementDelay)
			{
				return;
			}

			SetDelta();
			_aiMovementTime = 0f;

			_movementDelay = Random.Shared.Next(MinMovementDelay, MaxMovementDelay);
		}

		public void Update(float deltaTime)
		{
			_aiMovementTime += deltaTime;
		}

		private void SetDelta()
		{
			float angle = Random.Shared.NextSingle() * MathF.PI * 2;

			Delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}