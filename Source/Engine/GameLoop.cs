using Source.Engine.GameObjects;

namespace Source.Engine
{
	public class GameLoop
	{
		private const int TargetFramerate = 120;
		private const float TimeUntilUpdate = 1f / TargetFramerate;

		public static GameLoop Instance { get; private set; }

		private List<GameObject> _gameObjects = new();
		private List<GameObject> _newGameObjects = new();

		private List<IInpputHandler> _inputhandlers = new();

		private readonly PlayerInput _input;
		private readonly BaseRenderer _renderer;
		private readonly BaseGame _game;
		private readonly Time _time;

		private bool _isStopped = false;

		public GameLoop(PlayerInput input, BaseRenderer renderer, BaseGame game)
		{
			Instance = this;

			_renderer = renderer;
			_game = game;
			_input = input;

			_time = new();

			_gameObjects.Add(game);
			_inputhandlers.Add(game);
		}

		public void Run()
		{
			while (!IsEndGameLoop())
			{
				_renderer.Render();
				UpdateInput();
				Start();
				Update();
			}
		}

		public void Stop()
		{
			_isStopped = true;
		}

		private void UpdateInput()
		{
			_input.UpdateInput();

			foreach (var inputHandler in _inputhandlers)
			{
				inputHandler.UpdateInput();
			}
		}

		private void Update()
		{
			_time.Update();

			if (_time.BeforeUpdateTime < TimeUntilUpdate)
			{
				return;
			}

			_time.Reset();

			foreach (var updatable in _gameObjects)
			{
				if (updatable.IsActive)
				{
					updatable.Update(_time.DeltaTime);
				}		
			}
		}

		private void Start()
		{
			if (_newGameObjects.Count <= 0)
			{
				return;
			}

			foreach (var item in _newGameObjects)
			{
				StartNewGameObject(item);
			}

			_newGameObjects.Clear();
		}

		private bool IsEndGameLoop()
		{
			return !_isStopped && _game.IsEndGame();
		}

		private void StartNewGameObject(GameObject gameObject)
		{
			gameObject.Start();

			_gameObjects.Add(gameObject);

			if (gameObject is IInpputHandler inputGameObject && !_inputhandlers.Contains(inputGameObject))
			{
				_inputhandlers.Add(inputGameObject);
			}
		}



		public void Register<T>(T gameObject) where T : GameObject
		{
			if (_gameObjects.Contains(gameObject) || _newGameObjects.Contains(gameObject))
			{
				return;
			}

			_newGameObjects.Add(gameObject);
		}

		public void UnRegister<T>(T gameObject) where T : GameObject
		{
			_gameObjects.Remove(gameObject);
			_newGameObjects.Remove(gameObject);

			if (gameObject is IInpputHandler inputGameObject)
			{
				_inputhandlers.Remove(inputGameObject);
			}
		}
	}
}