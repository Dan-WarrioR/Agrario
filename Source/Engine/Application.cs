using Source.Engine.Configs;
using SFML.Graphics;
using Source.Engine.Input;
using Source.Game;

namespace Source.Engine
{
    public class Application
	{
		public void Run(IEnumerable<BaseConfig> configs)
		{
			InitializeConfigs(configs);

			RenderWindow window = CreateWindow();
			
			var game = new AgarioGame();
			var renderer = new SFMLRenderer(window);
			var input = new PlayerInput(window);
			var gameLoop = new GameLoop(input, renderer, game);

			window.Closed += (_, _) => gameLoop.Stop();
			
			game.Initialize(window, renderer);

			gameLoop.Run();
		}

		private void InitializeConfigs(IEnumerable<BaseConfig> configs)
		{
			ConfigManager configManager = new();

			configManager.RegisterConfigs(configs);

			configManager.LoadConfigs();
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