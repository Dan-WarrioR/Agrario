using Source.Engine.Configs;
using SFML.Graphics;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Game
{
    public class SFMLRenderer : BaseRenderer
	{
		public GameCamera Camera { get; private set; }

		public override void Initialize(RenderWindow window)
		{
			base.Initialize(window);

			Dependency.Register(this);

			Camera = new(window, WindowConfig.WindowSize, WindowConfig.WindowSize);
		}

		public override void Render()
		{
			Window.Clear(WindowConfig.BackgroundColor);
			Camera.Update();

			Camera.BeginGameView();
			DrawActiveElements(GameElements);

			Camera.BeginUIView();
			DrawActiveElements(UIElements);

			Window.Display();
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
					Window.Draw(obj);
				}
			}
		}
	}
}