using SFML.Graphics;
using Source.Engine;
using Source.Engine.Factory;
using Source.Game.Units;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private const int PlayersCount = 50;
		private const int FoodCount = 500;

		private GameLoop GameLoop => GameLoop.Instance;
		private RenderWindow _window;

		private UnitFactory _unitFactory;

		private List<Player> _players = new();

		private List<Food> _foods = new();

		private TextObject _text;

		private Player _mainPlayer;

		public AgarioGame()
		{

		}

		public void Initialize(RenderWindow window, UnitFactory unitFactory)
		{
			_window = window;
			_unitFactory = unitFactory;

			SpawnFood();

			SpawnBots();

			SpawnMainPlayer();

			SpawnUserUI();
		}

		private void SpawnBots()
		{
			for (int i = _players.Count; i < PlayersCount - 1; i++)
			{
				var bot = _unitFactory.CreatePlayer(true);

				_players.Add(bot);
			}		
		}

		private void SpawnFood()
		{
			for (int i = _foods.Count; i < FoodCount; i++)
			{
				var food = _unitFactory.CreateFood();

				_foods.Add(food);
			}
		}

		private void SpawnMainPlayer()
		{
			_mainPlayer = _unitFactory.CreatePlayer(false);

			//_camera = new(_window, _mainPlayer.Position, _bounds.Size);

			_players.Add(_mainPlayer);
		}

		private void SpawnUserUI()
		{
			_text = _unitFactory.CreateText(_mainPlayer.Mass.ToString());

			_mainPlayer.OnAteFood += _text.OnScoreChanged;
		}


		public override void UpdateInput()
		{
			_window.DispatchEvents();
		}


		public override void Update(float deltaTime)
		{
			//_camera.Update(_mainPlayer.Position);

			CheckFoodColissions();

			CheckPlayerColissions();

			CheckForGameRestart();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			//_camera.BeginGameView();

			//_camera.BeginUIView();
			_text.Draw(target, states);

			//_camera.BeginGameView();
		}



		public bool IsEndGame()
		{
			return false;
		}



		private void CheckForGameRestart()
		{
			if (_players.Count > 1)
			{
				return;
			}
			return;
			RestartGame();
		}

		private void CheckFoodColissions()
		{
			for (int i = 0; i < _players.Count; i++)
			{
				for (int j = 0; j < _foods.Count; j++)
				{
					var player = _players[i];
					var food = _foods[j];

					if (player.TryEat(food))
					{
						food.Dispose();

						_foods.Remove(food);

						continue;
					}
				}
			}
		}

		private void CheckPlayerColissions()
		{
			for (int i = _players.Count - 1; i >= 0; i--)
			{
				for (int j = _players.Count - 1; j >= 0; j--)
				{
					if (i == j)
					{
						continue;
					}

					var player = _players[i];
					var otherPlayer = _players[j];

					if (player.TryEat(otherPlayer))
					{
						otherPlayer.Dispose();
						_players.RemoveAt(j);

						if (j < i)
						{
							i--;
						}
					}
				}
			}
		}

		private void RestartGame()
		{
			foreach (var player in _players)
			{
				player.Dispose();
			}

			_mainPlayer.OnAteFood -= _text.OnScoreChanged;

			foreach (var food in _foods)
			{
				food.Dispose();
			}

			_players.Clear();
			_foods.Clear();

			SpawnFood();
			SpawnBots();
			SpawnMainPlayer();

			_mainPlayer.OnAteFood += _text.OnScoreChanged;
		}
	}
}