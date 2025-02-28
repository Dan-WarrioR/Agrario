using SFML.Graphics;
using Source.Engine.GameObjects;

namespace Source.Engine
{
	public abstract class BaseGameRules : GameObject
	{
		private bool _isEndGame = false;

		public abstract void Initialize();

		public override void Draw(RenderTarget target, RenderStates states) { }

		public virtual bool IsEndGame()
		{
			return _isEndGame;
		}

		public void StopGame()
		{
			_isEndGame = true;
		}
	}
}
