using SFML.Graphics;
using SFML.System;
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

			_camera = new(_window, WindowConfig.WindowSize, WindowConfig.WindowSize);
		}

		public void UpdateView(Vector2f playerPosition) //Clean with Dependency
		{
			_camera.Update(playerPosition);
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

			DrawUIElements();

			_window.Display();
		}

		private void DrawUIElements()
		{
			_camera.BeginUIView();

			foreach (var drawable in UIElements)
			{
				if (drawable.IsActive)
				{
					_window.Draw(drawable);
				}
			}
		}
	}
}