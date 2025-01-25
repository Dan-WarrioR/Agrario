using SFML.Graphics;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Game.Configs;

namespace Source.Game
{
	public class SFMLRenderer : BaseRenderer
	{
		private RenderWindow _window;

		public GameCamera Camera { get; private set; }

		public SFMLRenderer(RenderWindow window)
		{
			_window = window;

			Camera = new(_window, WindowConfig.WindowSize, WindowConfig.WindowSize);
		}

		public override void Render()
		{
			_window.Clear(WindowConfig.BackgroundColor);
			Camera.Update();

			Camera.BeginGameView();
			DrawActiveElements(GameElements);

			Camera.BeginUIView();
			DrawActiveElements(UIElements);

			_window.Display();
		}

		public void Zoom(float factor)
		{
			Camera.Zoom(factor);
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