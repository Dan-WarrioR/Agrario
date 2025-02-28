using SFML.Graphics;
using Source.Engine.Input;
using SFML.Window;
using Source.Engine.Rendering;
using Source.Engine.Systems;

namespace Source.Engine
{
    public class Application
	{
		public void Run(BaseGameRules gameRules, BaseRenderer renderer)
		{
			RenderWindow window = CreateWindow();

			renderer.Initialize(window);
			var input = new PlayerInput(window);
			var audioManager = new AudioManager();
			audioManager.LoadSounds();

			CreateSubSystems();
			
			var gameLoop = new GameLoop(input, renderer, gameRules);

			window.Closed += (_, _) => gameLoop.Stop();

			gameRules.Initialize();

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

		private void CreateSubSystems()
		{
			new EventBus();
			new PauseManager();
		}
	}
}