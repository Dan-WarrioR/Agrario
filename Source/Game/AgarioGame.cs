using Source.Game.Factories;
using SFML.Graphics;
using Source.Engine;
using Source.Game.Units;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private const int PlayersCount = 50;
		private const int FoodCount = 500;

		private GameLoop GameLoop => GameLoop.Instance;
		private RenderWindow _window;
		private SFMLRenderer _renderer;

		private UnitFactory _unitFactory;
		private UIFactory _uiFactory;

		private TextObject _text;
		private Player _mainPlayer;

		private List<Player> _players = new();
		private List<Food> _foods = new();

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

			//SpawnBots();

			SpawnMainPlayer();

			SpawnUserUI();
		}

		private void SpawnBots()
		{
			for (int i = 0; i < PlayersCount - 1; i++)
			{
				var bot = _unitFactory.SpawnBot();

				_players.Add(bot);
			}		
		}

		private void SpawnFood()
		{
			for (int i = 0; i < FoodCount; i++)
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
			_text = _uiFactory.CreateText(_mainPlayer.Mass.ToString());

			_mainPlayer.OnAteFood += _text.OnScoreChanged;
		}


		public override void UpdateInput()
		{
			_window.DispatchEvents();
		}


		public override void Update(float deltaTime)
		{
			_renderer.UpdateView(_mainPlayer.Position);

			CheckFoodColissions();

			return;

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
			return;

			RestartGame();
		}

		private void CheckFoodColissions()
		{
			foreach (var player in _players)
			{
				foreach (var food in _foods)
				{
					if (food.IsActive && player.TryEat(food))
					{
						food.SetActive(false);
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