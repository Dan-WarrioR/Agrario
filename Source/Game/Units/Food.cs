using SFML.Graphics;
using SFML.System;
using Source.Engine.Gameobjects;

namespace Source.Game.Units
{
	public interface IFood
	{
		public float Mass { get; }

		public float Radius { get; }

		public Vector2f Position { get; }
	}

	public class Food : CircleObject, IFood
	{
		private const float MassMultiplier = 1f;

		protected override Color FillColor => Color.Green;

		public float Mass => Radius * Radius * MathF.PI * MassMultiplier;

		public Food(float radius, Vector2f initialPosition) : base(radius, initialPosition)
		{

		}
	}
}