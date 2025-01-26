﻿using SFML.Graphics;
using SFML.System;
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

		private float _initialRadius;

		public void Initialize(Color color, float radius, Vector2f initialPosition)
		{
			Initialize(radius, initialPosition);
			_initialRadius = radius;

			Circle.FillColor = color;
		}	

		public void Reset()
		{
			SetActive(true);
			Circle.Radius = _initialRadius;
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
	}
}