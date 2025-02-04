using Source.Engine.Configs;
using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;
using Source.Game.Configs;

namespace Source.Game.Units
{
    public class Player : CircleObject, IEater, IFood
	{
		private const float BaseZoom = 0.1f;
		private const float ZoomFactorCoefficient = 0.01f;

		public event Action<float> OnAteFood;

		public float ZoomFactor
		{
			get
			{
				return BaseZoom + (Radius * ZoomFactorCoefficient);
			}
		}

		public float Mass => Radius * Radius * MathF.PI;

		public float CurrentSpeed
		{
			get
			{
				float speedReduction = (Radius - _speedReductionCoefficient) / 100f;
				
				return MathF.Max(_minSpeed, _baseSpeed * (1 - speedReduction));
			}
		}

		public event Action OnBeingEaten;

		private Vector2f Delta => _controller.Delta;

		private BaseController _controller;

		private static float _speedReductionCoefficient;
		private static float _minSpeed;
		private static float _baseSpeed;
		private static Vector2f _mirroredPlayerScale;
		private static Vector2f _normalPlayerScale;
		private static FloatRect _bounds;
		private static float _growthBase;
		private static float _growthDecayRate;
		private static float _minGrowthFactor;

		public void Initialize(BaseController controller, Color color, float radius, Vector2f initialPosition)
		{
			Initialize(radius, initialPosition);

			Circle.FillColor = color;
			Circle.OutlineThickness = 1f;
			Circle.OutlineColor = Color.Cyan;

			SetupConfigValues();

			SetConrtoller(controller);
		}	

		private void SetupConfigValues()
		{
			_speedReductionCoefficient = PlayerConfig.SpeedReductionCoefficient;
			_minSpeed = PlayerConfig.MinSpeed;
			_baseSpeed = PlayerConfig.BaseSpeed;
			_mirroredPlayerScale = PlayerConfig.MirroredPlayerScale;
			_normalPlayerScale = PlayerConfig.NormalPlayerScale;
			_bounds = WindowConfig.Bounds;
			_growthBase = PlayerConfig.GrowthBase;
			_growthDecayRate = PlayerConfig.GrowthDecayRate;
			_minGrowthFactor = PlayerConfig.MinGrowthFactor;
		}

		public void Reset()
		{
			SetActive(true);
			Circle.Radius = InitialRadius;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			Vector2f positionDelta = CurrentSpeed * deltaTime * Delta;

			var position = GetClampedPosition(Position + positionDelta);

			if (Delta.X > 0)
			{
				SetScale(_mirroredPlayerScale);
			}
			else if (Delta.X < 0)
			{
				SetScale(_normalPlayerScale);
			}

			SetPosition(position);
		}

		public void SetConrtoller(BaseController conrtoller)
		{
			_controller = conrtoller;
			conrtoller.SetTarget(this);
		}

		public void SwapControllers(Player otherPlayer)
		{
			var otherController = otherPlayer._controller;

			otherPlayer.SetConrtoller(_controller);
			SetConrtoller(otherController);
		}

		#region Eat

		public bool TryEat(IFood food)
		{
			if (!food.CanBeEatenBy(this))
			{
				return false;
			}

			food.EatMe();

			float growthFactor = _growthBase / (1f + _growthDecayRate * MathF.Log(Mass + 1));
			growthFactor = MathF.Max(growthFactor, _minGrowthFactor);

			float newMass = Mass + food.Mass * growthFactor;

			float newRadius = MathF.Sqrt(newMass / MathF.PI);

			Circle.Radius = newRadius;

			Circle.Origin = new(newRadius, newRadius);

			OnAteFood?.Invoke(Mass);

			return true;
		}

		public void EatMe()
		{
			SetActive(false);

			OnBeingEaten?.Invoke();
		}

		public bool CanBeEatenBy(Player player)
		{
			float distanceSquared = Position.DistanceToSquared(player.Position);
			float radiusDifference = player.Radius - Radius;
			
			return player.Radius > Radius && distanceSquared < radiusDifference * radiusDifference && IsActive;
		}

		#endregion			

		private Vector2f GetClampedPosition(Vector2f position)
		{
			float x = Math.Clamp(position.X, _bounds.Left, _bounds.Width);
			float y = Math.Clamp(position.Y, _bounds.Top, _bounds.Height);

			return new(x, y);
		}
	}
}