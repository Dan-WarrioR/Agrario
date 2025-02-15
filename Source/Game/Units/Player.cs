using Source.Engine.Configs;
using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;
using Source.Game.Configs;
using Source.Engine.Systems;

namespace Source.Game.Units
{
    public class Player : CircleObject, IEater, IFood
	{
		private const float BaseZoom = 0.1f;
		private const float ZoomFactorCoefficient = 0.01f;

		private PauseManager PauseManager => _pauseManager ??= Dependency.Get<PauseManager>();
		private PauseManager _pauseManager;

		public static Vector2f IdleDelta { get; private set; }

		private static float SpeedReductionCoefficient;
		private static float MinSpeed;
		private static float BaseSpeed;
		private static Vector2f MirroredPlayerScale;
		private static Vector2f NormalPlayerScale;
		private static FloatRect Bounds;
		private static float GrowthBase;
		private static float GrowthDecayRate;
		private static float MinGrowthFactor;

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

		public event Action OnBeingEaten;

		public Vector2f Delta => _controller.Delta;

		private BaseController _controller;	

		public void Initialize(BaseController controller, Color color, float radius, Vector2f initialPosition)
		{
			Initialize(radius, initialPosition);

			IdleDelta = new(0, 0);

			Circle.FillColor = color;
			Circle.OutlineColor = Color.Cyan;

			SetupConfigValues();

			SetConrtoller(controller);
		}	

		private void SetupConfigValues()
		{
			SpeedReductionCoefficient = PlayerConfig.SpeedReductionCoefficient;
			MinSpeed = PlayerConfig.MinSpeed;
			BaseSpeed = PlayerConfig.BaseSpeed;
			MirroredPlayerScale = PlayerConfig.MirroredPlayerScale;
			NormalPlayerScale = PlayerConfig.NormalPlayerScale;
			Bounds = WindowConfig.Bounds;
			GrowthBase = PlayerConfig.GrowthBase;
			GrowthDecayRate = PlayerConfig.GrowthDecayRate;
			MinGrowthFactor = PlayerConfig.MinGrowthFactor;
		}

		public void Reset()
		{
			SetActive(true);
			Circle.Radius = InitialRadius;
		}

		public override void OnUpdate(float deltaTime)
		{
			if (PauseManager.IsPaused)
			{
				return;
			}

			Vector2f positionDelta = CurrentSpeed * deltaTime * Delta;

			var position = GetClampedPosition(Position + positionDelta);

			if (Delta.X > 0)
			{
				SetScale(MirroredPlayerScale);
			}
			else if (Delta.X < 0)
			{
				SetScale(NormalPlayerScale);
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

			float growthFactor = GrowthBase / (1f + GrowthDecayRate * MathF.Log(Mass + 1));
			growthFactor = MathF.Max(growthFactor, MinGrowthFactor);

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

		public bool IsMoving()
		{
			return Delta != IdleDelta;
		}

		private Vector2f GetClampedPosition(Vector2f position)
		{
			float x = Math.Clamp(position.X, Bounds.Left, Bounds.Width);
			float y = Math.Clamp(position.Y, Bounds.Top, Bounds.Height);

			return new(x, y);
		}
	}
}