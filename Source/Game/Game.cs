using SFML.Graphics;
using Source.Engine;
using Source.Engine.Factory;
using Source.Engine.Gameobjects;
using Source.Game.Units;

namespace Source.Game
{
	public class Game : IUpdatable
	{
		private const int PlayersCount = 50;
		private const int FoodCount = 500;

		private FloatRect _bounds;

		private List<Player> _players = new();

		private List<Food> _foods = new();

		private TextObject _mainPlayerScore;

		private Player _mainPlayer;

		public Game(FloatRect bounds)
		{
			_bounds = bounds;

			SpawnFood();

			SpawnBots();

			SpawnMainPlayer();

			SpawnUserUI();
		}

		private void SpawnBots()
		{
			for (int i = _players.Count; i < PlayersCount - 1; i++)
			{
				var bot = UnitFactory.CreatePlayer(true, _bounds);

				_players.Add(bot);
			}		
		}

		private void SpawnFood()
		{
			for (int i = _foods.Count; i < FoodCount; i++)
			{
				var food = UnitFactory.CreateFood(_bounds);

				_foods.Add(food);
			}
		}

		private void SpawnMainPlayer()
		{
			_mainPlayer = UnitFactory.CreatePlayer(false, _bounds);

			_players.Add(_mainPlayer);
		}

		private void SpawnUserUI()
		{
			_mainPlayerScore = UnitFactory.CreateText(_mainPlayer.Mass.ToString(), _bounds);

			_mainPlayer.OnAteFood += _mainPlayerScore.OnScoreChanged;
		}

	

		public void Update(float deltaTime)
		{
			CheckFoodColissions();

			CheckPlayerColissions();

			CheckForGameRestart();
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

			_mainPlayer.OnAteFood -= _mainPlayerScore.OnScoreChanged;

			foreach (var food in _foods)
			{
				food.Dispose();
			}

			_players.Clear();
			_foods.Clear();

			SpawnFood();
			SpawnBots();
			SpawnMainPlayer();

			_mainPlayer.OnAteFood += _mainPlayerScore.OnScoreChanged;
		}
	}
}