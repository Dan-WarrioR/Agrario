using SFML.System;
using Source.Engine.GameObjects;

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
	}
}