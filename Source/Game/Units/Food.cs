using SFML.Graphics;
using SFML.System;
using Source.Engine.Tools;

namespace Source.Game.Units
{
    public interface IFood
	{
		public float Mass { get; }

		public bool CanBeEatenBy(Player player);
	}

	public class Food : CircleObject, IFood
	{
		private static readonly Color FoodColor = new(241, 196, 15);

		private const float MassMultiplier = 1f;

		protected override Color FillColor => FoodColor;

		public float Mass => Radius * Radius * MathF.PI * MassMultiplier;

		public Food(float radius, Vector2f initialPosition) : base(radius, initialPosition)
		{

		}

		public void Eat()
		{
			SetActive(false);
		}

		public bool CanBeEatenBy(Player player)
		{
			float distance = Position.DistanceTo(player.Position);

			return player.Radius > Radius && distance + Radius < player.Radius && IsActive;
		}
	}
}