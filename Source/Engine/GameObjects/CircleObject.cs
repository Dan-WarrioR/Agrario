using Agrario.Source.Engine.GameObjects;
using SFML.Graphics;
using SFML.System;
using Source.Engine.Tools;

namespace Source.Game.Units
{
    public class CircleObject : ShapeObject
    {
        public float Radius => Circle.Radius;

        protected CircleShape Circle { get; private set; }

        public void Initialize(float radius, Vector2f initialPosition)
        {
            Initialize(new CircleShape(radius), initialPosition);

			Circle = (CircleShape)Shape;

			Shape.Origin = new(radius, radius);
		}

		public bool IsIntersects(CircleObject sphere)
        {
            float distanceToObject = Position.DistanceTo(sphere.Position);
            
            return distanceToObject <= Radius + sphere.Radius;
        }
    }
}
