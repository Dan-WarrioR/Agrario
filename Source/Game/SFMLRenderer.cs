using SFML.Graphics;
using Source.Engine;
using Source.Game.Configs;

namespace Source.Game
{
	public class SFMLRenderer : BaseRenderer
	{
		private RenderWindow _window;

		private GameCamera _camera;

		public SFMLRenderer(RenderWindow window)
		{
			_window = window;

			_camera = new(window, new(0,0), WindowConfig.WindowSize);
		}

		public override void Render()
		{
			_window.Clear(WindowConfig.BackgroundColor);

			_camera.BeginGameView();
			foreach (var drawable in GameElements)
			{
				if (drawable.IsActive)
				{
					_window.Draw(drawable);
				}	
			}

			_camera.BeginUIView();
			foreach (var drawable in UIElements)
			{
				if (drawable.IsActive)
				{
					_window.Draw(drawable);
				}
			}

			_window.Display();
		}
	}
}