using SFML.Graphics;
using SFML.System;

namespace Source.Engine
{
	public class GameLoop
	{
		private static List<IUpdatable> _updatables = new();

		private static List<Drawable> _drawables = new();

		private static List<IInpputHandler> _inputhandlers = new();

		private readonly RenderWindow _window;

		private readonly Clock _clock;

		private Game.Game _game;

		public GameLoop(RenderWindow window)
		{
			_window = window;

			FloatRect windowBounds = new FloatRect(0, 0, _window.Size.X, _window.Size.Y);

			_clock = new();

			_game = new(windowBounds);

			_updatables.Add(_game);
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
			_window.DispatchEvents();

			for (int i = 0; i < _inputhandlers.Count; i++)
			{
				_inputhandlers[i].UpdateInput();
			}
		}

		private void Update()
		{
			var deltaTime = _clock.Restart().AsSeconds();

			for (int i = 0; i < _updatables.Count; i++)
			{
				_updatables[i].Update(deltaTime);
			}
		}

		private void Draw()
		{
			_window.Clear(Color.White);

			for (int i = 0; i < _drawables.Count; i++)
			{
				_window.Draw(_drawables[i]);
			}

			_window.Display();
		}



		private bool IsEndGameLoop()
		{
			return !_game.IsEndGame() && !_window.IsOpen;
		}



		public static void Register(GameObject gameObject)
		{
			_drawables.Add(gameObject);
			_updatables.Add(gameObject);

			if (gameObject is IInpputHandler inputGameObject)
			{
				_inputhandlers.Add(inputGameObject);
			}
		}

		public static void UnRegister(GameObject gameObject)
		{
			_drawables.Remove(gameObject);
			_updatables.Remove(gameObject);
			
			if (gameObject is IInpputHandler inputGameObject)
			{
				_inputhandlers.Remove(inputGameObject);
			}
		}
	}
}