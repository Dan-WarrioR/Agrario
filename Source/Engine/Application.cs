using SFML.Graphics;
using Source.Game;
using Source.Game.Configs;

namespace Source.Engine
{
	public class Application
	{
		public void Run()
		{
			RenderWindow window = CreateWindow();
			
			var game = new AgarioGame();
			var renderer = new SFMLRenderer(window);
			var input = new PlayerInput(window);
			var gameLoop = new GameLoop(input, renderer, game);

			window.Closed += (_, _) => gameLoop.Stop();
			
			game.Initialize(window, renderer, input);

			gameLoop.Run();
		}

		private RenderWindow CreateWindow()
		{
			var window = new RenderWindow(WindowConfig.VideoMode, WindowConfig.AppName, WindowConfig.Style);

			window.SetFramerateLimit(60);
			window.SetVerticalSyncEnabled(true);

			window.Closed += (_, _) => window.Close();

			return window;
		}
	}
}