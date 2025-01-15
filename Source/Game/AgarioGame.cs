using Source.Game.Factories;
using SFML.Graphics;
using Source.Engine;
using Source.Game.Units;
using Source.Game.Configs;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private GameLoop GameLoop => GameLoop.Instance;
		private RenderWindow _window;
		private SFMLRenderer _renderer;

		private UnitFactory _unitFactory;
		private UIFactory _uiFactory;

		private TextObject _scoreText;
		private TextObject _countText;

		private Player _mainPlayer;

		private List<Food> _foods = new(GameConfig.FoodCount);
		private List<Player> _players = new(GameConfig.PlayersCount);

		private int _alivedPlayersCount = 0;

		public AgarioGame()
		{
			
		}

		public void Initialize(RenderWindow window, SFMLRenderer renderer)
		{
			_window = window;
			_renderer = renderer;

			_unitFactory = new(GameLoop, renderer);
			_uiFactory = new(GameLoop, renderer);

			SpawnFood();

			SpawnBots();

			SpawnMainPlayer();

			_alivedPlayersCount = _players.Count;

			SpawnUserUI();
		}

		private void SpawnBots()
		{
			for (int i = 0; i < GameConfig.PlayersCount - 1; i++)
			{
				var bot = _unitFactory.SpawnBot();

				_players.Add(bot);
			}
		}

		private void SpawnFood()
		{
			for (int i = 0; i < GameConfig.FoodCount; i++)
			{
				var food = _unitFactory.SpawnFood();
				_foods.Add(food);
			}
		}

		private void SpawnMainPlayer()
		{
			_mainPlayer = _unitFactory.SpawnPlayer();

			_players.Add(_mainPlayer);
		}

		private void SpawnUserUI()
		{
			_scoreText = _uiFactory.CreateScoreText(_mainPlayer.Mass.ToString());

			_countText = _uiFactory.CreateCountText(_alivedPlayersCount.ToString());

			_mainPlayer.OnAteFood += UpdateScore;
		}


		public override void UpdateInput()
		{
			_window.DispatchEvents();
		}


		public override void Update(float deltaTime)
		{
			_renderer.UpdateView(_mainPlayer.Position);

			CheckFoodColissions();

			CheckPlayerColissions();

			_countText.ChangeText(_alivedPlayersCount.ToString());

			CheckForGameRestart();
		}



		public bool IsEndGame()
		{
			return false;
		}



		private void CheckForGameRestart()
		{
			if (!_mainPlayer.IsActive || _alivedPlayersCount > 1)
			{
				return;
			}

			RestartGame();
		}

		private void CheckFoodColissions()
		{
			foreach (var player in _players)
			{
				foreach (var food in _foods)
				{
					if (player.TryEat(food))
					{
						food.Eat();
					}
				}		
			}
		}

		private void CheckPlayerColissions()
		{
			for (int i = 1; i < _players.Count; i++)
			{
				var bot1 = _players[i - 1];
				var bot2 = _players[i];

				if (bot1.IsActive && bot1.TryEat(bot2))
				{
					EatPlayer(bot2);
				}
				else if (bot2.IsActive && bot2.TryEat(bot1))
				{
					EatPlayer(bot1);
				}
			}



			for (int i = 0; i < _players.Count; i++)
			{
				var player1 = _players[i];

				if (!player1.IsActive)
				{
					continue;
				}

				for (int j = i + 1; j < _players.Count; j++)
				{
					var player2 = _players[j];

					if (!player2.IsActive)
					{
						continue;
					}

					if (player1.TryEat(player2))
					{
						EatPlayer(player2);
					}
					else if (player2.TryEat(player1))
					{
						EatPlayer(player1);
					}
				}
			}
		}



		private void RestartGame()
		{
			foreach (var food in _foods)
			{
				_unitFactory.RespawnFood(food);
			}

			foreach (var player in _players)
			{
				_unitFactory.RespawnPlayer(player);
			}	

			_alivedPlayersCount = _players.Count;
		}

		private void EatPlayer(Player player)
		{
			player.SetActive(false);
			_alivedPlayersCount--;
		}

		private void UpdateScore(float mass)
		{
			_scoreText.ChangeText(mass.ToString());
		}
	}
}