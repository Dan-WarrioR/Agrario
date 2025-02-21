using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Tools;

namespace Source.Engine
{
    public class GameLoop
	{
		private const int TargetFramerate = 120;
		private const float TimeUntilUpdate = 1f / TargetFramerate;

		private List<GameObject> _gameObjects = new();
		private List<GameObject> _newGameObjects = new();
		private List<GameObject> _destroyedGameObjects = new();

		private readonly PlayerInput _input;
		private readonly BaseRenderer _renderer;
		private readonly BaseGame _game;
		private readonly Time _time;

		private bool _isStopped = false;

		public GameLoop(PlayerInput input, BaseRenderer renderer, BaseGame game)
		{
			Dependency.Register(this);

			_renderer = renderer;
			_game = game;
			_input = input;

			_time = new();

			Register(game);
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
			_input.UpdateInputStates();
		}

		private void Update()
		{
			_time.Update();
			_input.InvokeBindings();

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

				if (updatable.IsDestroyed)
				{
					_destroyedGameObjects.Add(updatable);
				}
			}

			foreach (var destroyedObject in _destroyedGameObjects)
			{
				DestroyGameObject(destroyedObject);
			}

			_destroyedGameObjects.Clear();
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
		}

		private void DestroyGameObject(GameObject gameObject)
		{
			_gameObjects.Remove(gameObject);
			_renderer.RemoveGameElement(gameObject);
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
		}
	}
}