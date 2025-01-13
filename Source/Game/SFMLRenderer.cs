using SFML.Graphics;
using Source.Engine;
using Source.Game.Configs;

namespace Source.Game
{
	public class SFMLRenderer : BaseRenderer
	{
		private RenderWindow _window;

		public SFMLRenderer(RenderWindow window)
		{
			_window = window;
		}

		public override void Render()
		{
			_window.Clear(WindowConfig.BackgroundColor);

			foreach (var drawable in Drawables)
			{
				_window.Draw(drawable);
			}

			_window.Display();
		}
	}
}
