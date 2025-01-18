using SFML.Graphics;
using Source.Engine.GameObjects;

namespace Source.Engine
{
	public abstract class BaseGame : GameObject, IInpputHandler
	{
		public override void Draw(RenderTarget target, RenderStates states) { }

		public abstract void UpdateInput();

		public virtual bool IsEndGame()
		{
			return false;
		}
	}
}
