using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Game.Units
{
    public class CircleObject : GameObject
    {
        protected virtual Color FillColor => Color.White;

        public FloatRect ObjectRect => Circle.GetGlobalBounds();

        public Vector2f Position => Circle.Position;

        public float Radius => Circle.Radius;

        protected CircleShape Circle { get; }

        public CircleObject(float radius, Vector2f initialPosition)
        {
            Circle = new CircleShape(radius)
            {
                Position = initialPosition,
                Origin = new(radius, radius),
                FillColor = FillColor,
            };
        }

		public void SetPosition(Vector2f position)
		{
			Circle.Position = position;
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
            Circle.Draw(target, states);
        }
    }
}
