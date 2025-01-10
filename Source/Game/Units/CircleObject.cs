using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Tools;

namespace Source.Game.Units
{
	public class CircleObject : GameObject
	{
		protected virtual Color FillColor => Color.White;

		public FloatRect ObjectRect => Shape.GetGlobalBounds();

		public override Vector2f Position => Shape.Position;

		public float Radius { get; }

		protected Shape Shape { get; }

		public CircleObject(float radius, Vector2f initialPosition) : base(initialPosition)
		{
			Radius = radius;

			Shape = new CircleShape(radius)
			{
				Position = initialPosition,
				Origin = new(Radius, Radius),
				FillColor = FillColor,
			};
		}

		public bool IsIntersects(CircleObject sphere)
		{
			float distanceToObject = Position.DistanceTo(sphere.Position);

			return distanceToObject <= Radius + sphere.Radius;
		}

		public bool IsIntersects(FloatRect objectRect)
		{
			return ObjectRect.Intersects(objectRect);
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			Shape.Draw(target, states);
		}
	}
}
