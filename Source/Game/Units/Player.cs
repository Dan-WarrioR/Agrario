using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Engine.Tools;
using Source.Game.Configs;
using Source.Game.Units.Components.Input;

namespace Source.Game.Units
{
	public interface IEater
	{
		public bool TryEat(IFood food);
	}

	public class Player : CircleObject, IEater, IFood, IInpputHandler
	{
		private const float BaseZoom = 0.1f;
		private const float ZoomFactorCoefficient = 0.01f;

		private const float GrowthBase = 2f;
		private const float GrowthDecayRate = 0.5f;
		private const float MinGrowthFactor = 1f;

		private const float BaseSpeed = 200f;
		private const float MinSpeed = 100f;
		private const float SpeedReductionCoefficient = 20f;

		private const float MassMultiplier = 1f;

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
				float speedReduction = (Radius - SpeedReductionCoefficient) / 100f;

				return MathF.Max(MinSpeed, BaseSpeed * (1 - speedReduction));
			}
		}

		public IInputComponent InputComponent;

		private float _initialRadius;

		public Player(IInputComponent inputComponent, Color color, float radius, Vector2f initialPosition) : base(radius, initialPosition)
		{
			InputComponent = inputComponent;
			_initialRadius = radius;

			Circle.FillColor = color;
		}

		public void ChangeInputComponent(IInputComponent inputComponent)
		{
			InputComponent = inputComponent;
		}

		public void Reset()
		{
			SetActive(true);
			Circle.Radius = _initialRadius;
		}

		public void UpdateInput()
		{
			if (!IsActive)
			{
				return;
			}

			InputComponent.UpdateInput();
		}

		public override void Update(float deltaTime)
		{
			if (!IsActive)
			{
				return;
			}

			InputComponent.Update(deltaTime);

			Circle.Position += CurrentSpeed * deltaTime * InputComponent.Delta;

			ClampPosition();
		}

		#region Eat

		public bool TryEat(IFood food)
		{
			if (!food.CanBeEatenBy(this))
			{
				return false;
			}

			float growthFactor = GrowthBase / (1f + GrowthDecayRate * MathF.Log(Mass + 1));
			growthFactor = MathF.Max(growthFactor, MinGrowthFactor);

			float newMass = Mass + food.Mass * growthFactor;

			float newRadius = MathF.Sqrt(newMass / MathF.PI);

			Circle.Radius = newRadius;

			Circle.Origin = new(newRadius, newRadius);

			OnAteFood?.Invoke(Mass);

			return true;
		}

		public bool CanBeEatenBy(Player player)
		{
			float distanceSquared = Position.DistanceToSquared(player.Position);
			float radiusDifference = player.Radius - Radius;
			
			return player.Radius > Radius && distanceSquared < radiusDifference * radiusDifference && IsActive;
		}

		#endregion

		private void ClampPosition()
		{
			var bounds = WindowConfig.Bounds;

			float x = Math.Clamp(Position.X, bounds.Left, bounds.Width);
			float y = Math.Clamp(Position.Y, bounds.Top, bounds.Height);

			Circle.Position = new(x, y);
		}		
	}
}