using SFML.System;

namespace Source.Game.Units.Components.Input
{
	public class RandomDirectionInput : IInputComponent
	{
		private const float AiMovementDelay = 2f;

		public Vector2f Delta { get; private set; }

		private float _aiMovementTime = AiMovementDelay;

		public void UpdateInput()
		{
			if (_aiMovementTime >= AiMovementDelay)
			{
				SetDelta();
				_aiMovementTime = 0f;

				return;
			}
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