using Source.Engine.Configs;
using SFML.Graphics;
using Source.Engine.Input;

namespace Source.Engine
{
    public class Application
	{
		public void Run(BaseGame game, BaseRenderer renderer, IEnumerable<BaseConfig> configs)
		{
			InitializeConfigs(configs);

			RenderWindow window = CreateWindow();

			renderer.Initialize(window);
			var input = new PlayerInput(window);
			var gameLoop = new GameLoop(input, renderer, game);

			window.Closed += (_, _) => gameLoop.Stop();
			
			game.Initialize();

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