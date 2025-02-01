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
				float speedReduction = (Radius - PlayerConfig.SpeedReductionCoefficient) / 100f;

				return MathF.Max(PlayerConfig.MinSpeed, PlayerConfig.BaseSpeed * (1 - speedReduction));
			}
		}

		public event Action OnBeingEaten;

		private Vector2f _delta;

		private BaseController _controller;

		public void Initialize(BaseController controller, Color color, float radius, Vector2f initialPosition)
		{
			Initialize(radius, initialPosition);

			Circle.FillColor = color;

			SetConrtoller(controller);
		}	

		public void Reset()
		{
			SetActive(true);
			Circle.Radius = InitialRadius;
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			Vector2f positionDelta = CurrentSpeed * deltaTime * _delta;

			var position = GetClampedPosition(Position + positionDelta);

			if (_delta.X > 0)
			{
				SetScale(PlayerConfig.MirroredPlayerScale);
			}
			else if (_delta.X < 0)
			{
				SetScale(PlayerConfig.NormalPlayerScale);
			}

			SetPosition(position);
		}

		public void SetDelta(Vector2f delta)
		{
			_delta = delta;
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

			float growthFactor = PlayerConfig.GrowthBase / (1f + PlayerConfig.GrowthDecayRate * MathF.Log(Mass + 1));
			growthFactor = MathF.Max(growthFactor, PlayerConfig.MinGrowthFactor);

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
			var bounds = WindowConfig.Bounds;

			float x = Math.Clamp(position.X, bounds.Left, bounds.Width);
			float y = Math.Clamp(position.Y, bounds.Top, bounds.Height);

			return new(x, y);
		}
	}
}