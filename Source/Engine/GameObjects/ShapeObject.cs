using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;

namespace Agrario.Source.Engine.GameObjects
{
    public class ShapeObject : GameObject
    {
        protected virtual Color FillColor => Color.White;

        public FloatRect ObjectRect => Shape.GetGlobalBounds();

        public Vector2f Position => Shape.Position;

        protected Shape Shape { get; private set; }

        public void Initialize(Shape shape, Vector2f initialPosition)
        {
            Initialize(initialPosition);

            Shape = shape;

            shape.Position = initialPosition;

            var shapeBounds = shape.GetLocalBounds();
            shape.Origin = new Vector2f(shapeBounds.Width / 2, shapeBounds.Height / 2);
            shape.FillColor = FillColor;
        }

        public void SetPosition(Vector2f position)
        {
            Shape.Position = position;
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
