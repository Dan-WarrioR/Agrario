using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Tools;

namespace Source.Game.Units
{
	public struct MovementKey
	{
		public Keyboard.Key Key { get; }
		public float DeltaX { get; }
		public float DeltaY { get; }

		public MovementKey(Keyboard.Key key, float deltaX, float deltaY)
		{
			Key = key;
			DeltaX = deltaX;
			DeltaY = deltaY;
		}

		public bool IsPressed()
		{
			return Keyboard.IsKeyPressed(Key);
		}
	}

	public interface IEater
	{
		public bool TryEat(IFood food);
	}

	public class Player : CircleObject, IEater, IFood
	{
		private const float BaseSpeed = 200f;
		private const float MinSpeed = 100f;
		private const float SpeedReductionCoefficient = 20f;

		private const float StopDistance = 1f;
		private const float AiMovementDelay = 2f;

		private const float MassMultiplier = 1f;

		protected override Color FillColor => _isAi ? Color.Red : Color.Blue;

		public event Action<float> OnAteFood;

		public float Mass => Radius * Radius * MathF.PI * MassMultiplier;

		public float CurrentSpeed
		{
			get
			{
				float speedReduction = (Radius - SpeedReductionCoefficient) / 100f;

				return MathF.Max(MinSpeed, BaseSpeed * (1 - speedReduction));
			}
		}

		private bool _isAi;

		private float _aiMovementTime = 0f;
		private Vector2f _delta;
		private List<MovementKey> _keyMap = new(4);

		private Random _random = new();		

		private FloatRect _bounds;

		public Player(bool isAi, FloatRect bounds, float radius, Vector2f initialPosition) : base(radius, initialPosition)
		{
			_isAi = isAi;
			_bounds = bounds;

			Circle.FillColor = FillColor;

			if (_isAi)
			{
				GenerateRandomDirection();
			}
			else
			{
				_keyMap = new()
			{
				new(Keyboard.Key.W, 0, -1),
				new(Keyboard.Key.S, 0, 1),
				new(Keyboard.Key.A, -1, 0),
				new(Keyboard.Key.D, 1, 0),
			};
			}
		}

		public void UpdateInput()
		{
			if (_isAi)
			{
				if (_aiMovementTime >= AiMovementDelay)
				{
					GenerateRandomDirection();
					_aiMovementTime = 0f;
				}

				return;
			}

			_delta = GetDelta();
		}

		public override void Update(float deltaTime)
		{
			_aiMovementTime += deltaTime;

			Circle.Position += CurrentSpeed * deltaTime * _delta;

			ClampPosition();
		}

		//Eat	

		public bool TryEat(IFood food)
		{
			if (!CanEat(food))
			{
				return false;
			}

			float newArea = Mass + food.Mass;
			float newRadius = MathF.Sqrt(newArea / MathF.PI);

			Circle.Radius = newRadius;

			Circle.Origin = new(newRadius, newRadius);

			OnAteFood?.Invoke(Mass);

			return true;
		}

		private bool CanEat(IFood food)
		{
			return Radius > food.Radius && Position.DistanceTo(food.Position) < Radius;
		}

		//Position

		private void ClampPosition()
		{
			float x = Math.Clamp(Position.X, _bounds.Left, _bounds.Width);
			float y = Math.Clamp(Position.Y, _bounds.Top, _bounds.Height);

			Circle.Position = new(x, y);
		}

		private Vector2f GetDelta()
		{
			float deltaX = 0;
			float deltaY = 0;

			foreach (var key in _keyMap)
			{
				if (key.IsPressed())
				{
					deltaX += key.DeltaX;
					deltaY += key.DeltaY;
				}
			}

			return new(deltaX, deltaY);
		}

		private void GenerateRandomDirection()
		{
			float angle = _random.NextSingle() * MathF.PI * 2;

			_delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}