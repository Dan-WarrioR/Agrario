using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Source.Engine
{
	public class GameLoop
	{
		private static List<IUpdatable> _updatables;

		private static List<Drawable> _drawables;

		private readonly RenderWindow _window;

		private readonly Clock _clock;

		private Game.Game _game;

		public GameLoop(RenderWindow window)
		{
			_window = window;

			_clock = new();

			_game = new();
		}

		public void Run()
		{
			while (!IsEndGameLoop())
			{
				Draw();
				UpdateInput();
				Update();
			}
		}

		private void UpdateInput()
		{

		}

		private void Update()
		{
			var deltaTime = _clock.Restart().AsSeconds();

			foreach (var updatable in _updatables)
			{
				updatable.Update(deltaTime);
			}
		}

		private void Draw()
		{
			_window.Clear(Color.White);

			foreach (var drawable in _drawables)
			{
				_window.Draw(drawable);
			}

			_window.Display();
		}



		private bool IsEndGameLoop()
		{
			return !_game.IsEndGame() && !_window.IsOpen;
		}



		public static void Register(GameObject gameObject)
		{
			if (gameObject is Drawable)
			{
				_drawables.Add(gameObject);
			}

			if (gameObject is IUpdatable)
			{
				_updatables.Add(gameObject);
			}
		}

		public static void UnRegister(GameObject gameObject)
		{
			if (gameObject is Drawable)
			{
				_drawables.Remove(gameObject);
			}

			if (gameObject is IUpdatable)
			{
				_updatables.Remove(gameObject);
			}
		}
	}
}