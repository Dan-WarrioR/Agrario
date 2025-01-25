using SFML.System;
using Source.Engine.GameObjects;
using Source.Game.Configs;

namespace Source.Game.Units.Controllers
{
	public class BaseController : GameObject
	{
		protected GameObject Target { get; private set; }

		protected Vector2f Delta;

		public virtual void SetTarget(GameObject target)
		{
			Target = target;
		}

		protected Vector2f GetClampedPosition(Vector2f position)
		{
			var bounds = WindowConfig.Bounds;

			float x = Math.Clamp(position.X, bounds.Left, bounds.Width);
			float y = Math.Clamp(position.Y, bounds.Top, bounds.Height);

			return new(x, y);
		}
	}
}