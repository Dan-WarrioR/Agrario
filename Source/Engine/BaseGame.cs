using SFML.Graphics;
using Source.Engine.GameObjects;

namespace Source.Engine
{
	public abstract class BaseGame : GameObject
	{
		public abstract void Initialize();

		public override void Draw(RenderTarget target, RenderStates states) { }

		public virtual bool IsEndGame()
		{
			return false;
		}
	}
}
