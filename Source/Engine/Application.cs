using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Engine.Factory;
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

			var gameLoop = new GameLoop(renderer, game);

			window.Closed += (_, _) => gameLoop.Stop();

			var unitFactory = new UnitFactory(gameLoop, renderer);

			game.Initialize(window, unitFactory);

			gameLoop.Run();
		}

		private RenderWindow CreateWindow()
		{
			Vector2f windowSize = WindowConfig.WindowSize;

			var videoMode = new VideoMode((uint)windowSize.X, (uint)windowSize.Y);
			var window = new RenderWindow(videoMode, WindowConfig.AppName);

			window.SetFramerateLimit(60);
			window.SetVerticalSyncEnabled(true);

			window.Closed += (_, _) => window.Close();

			return window;
		}
	}
}