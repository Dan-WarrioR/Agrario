using SFML.Graphics;
using Source.Engine.Input;
using SFML.Window;
using Source.Engine.Configs;

namespace Source.Engine
{
    public class Application
	{
		public void Run(BaseGame game, BaseRenderer renderer)
		{
			RenderWindow window = CreateWindow();

			renderer.Initialize(window);
			var input = new PlayerInput(window);
			var gameLoop = new GameLoop(input, renderer, game);

			window.Closed += (_, _) => gameLoop.Stop();

			ConfigLoader.LoadConfig(typeof(WindowConfig));

			game.Initialize();

			gameLoop.Run();
		}

		private RenderWindow CreateWindow()
		{
			var window = new RenderWindow(VideoMode.DesktopMode, "Game", Styles.Fullscreen);

			window.SetFramerateLimit(60);
			window.SetVerticalSyncEnabled(true);

			window.Closed += (_, _) => window.Close();

			return window;
		}
	}
}