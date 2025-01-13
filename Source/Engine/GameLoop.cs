using Source.Engine.GameObjects;

namespace Source.Engine
{
	public class GameLoop
	{
		private const int TargetFramerate = 120;
		private const float TimeUntilUpdate = 1f / TargetFramerate;

		public static GameLoop Instance { get; private set; }

		private List<GameObject> _gameObjects = new();

		private List<IInpputHandler> _inputhandlers = new();

		private readonly BaseRenderer _renderer;
		private readonly BaseGame _game;
		private readonly Time _time;

		private bool _isActive = true;

		public GameLoop(BaseRenderer renderer, BaseGame game)
		{
			Instance = this;

			_renderer = renderer;
			_game = game;

			_time = new();

			_gameObjects.Add(game);
			_inputhandlers.Add(game);
		}

		public void Run()
		{
			while (_isActive)
			{
				_renderer.Render();
				UpdateInput();
				Start(); //StartForNewObjects()
				Update();
			}
		}

		private void UpdateInput()
		{
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

			for (int i = 0; i < _gameObjects.Count; i++)
			{
				_gameObjects[i].Update(_time.DeltaTime);
			}
		}

		private void Start()
		{
			
		}



		public void StopGameLoop()
		{
			_isActive = false;
		}

		public void Register<T>(T gameObject) where T : GameObject
		{
			if (!_gameObjects.Contains(gameObject))
			{
				_gameObjects.Add(gameObject);
			}		

			if (gameObject is IInpputHandler inputGameObject && !_inputhandlers.Contains(inputGameObject))
			{
				_inputhandlers.Add(inputGameObject);
			}
		}

		public void UnRegister<T>(T gameObject) where T : GameObject
		{
			_gameObjects.Remove(gameObject);
			
			if (gameObject is IInpputHandler inputGameObject)
			{
				_inputhandlers.Remove(inputGameObject);
			}
		}
	}
}