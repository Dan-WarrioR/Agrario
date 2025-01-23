using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;
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
			DrawActiveElements(GameElements);

			_camera.BeginUIView();
			DrawActiveElements(UIElements);

			_window.Display();
		}

		public void Zoom(float factor)
		{
			_camera.Zoom(factor);
		}

		private void DrawActiveElements<T>(IEnumerable<T> objects) where T : Drawable, IActivable
		{
			foreach (var obj in objects)
			{
				if (obj.IsActive)
				{
					_window.Draw(obj);
				}
			}
		}
	}
}