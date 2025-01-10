using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Source.Engine
{
	public class Application
	{
		private static readonly Vector2f WindowSize = new(1000, 500);

		private const string WindowAppName = "Air Hockey";

		public void Run()
		{
			RenderWindow window = CreateWindow();

			//EntityHandler entityHandler = new(window);

			GameLoop gameLoop = new(window);

			gameLoop.Run();
		}

		private RenderWindow CreateWindow()
		{
			var videoMode = new VideoMode((uint)WindowSize.X, (uint)WindowSize.Y);
			var window = new RenderWindow(videoMode, WindowAppName);

			window.SetFramerateLimit(60);
			window.SetVerticalSyncEnabled(true);

			window.Closed += (_, _) => window.Close();

			return window;
		}
	}
}