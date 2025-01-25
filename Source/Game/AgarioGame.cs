using Source.Game.Factories;
using SFML.Graphics;
using Source.Engine;
using Source.Game.Units;
using Source.Game.Configs;
using Source.Engine.Tools;
using Source.Game.Units.Components;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private GameLoop GameLoop => GameLoop.Instance;
		private RenderWindow _window;
		private SFMLRenderer _renderer;
		private PlayerInput _input;

		private UnitFactory _unitFactory;
		private UIFactory _uiFactory;

		private TextObject _scoreText;
		private TextObject _countText;

		private Player _mainPlayer;

		private List<Food> _foods = new(GameConfig.FoodCount);
		private List<Player> _players = new(GameConfig.PlayersCount);

		private event Action<int> OnPlayerDied; 

		private int _alivedPlayersCount = 0;

		public void Initialize(RenderWindow window, SFMLRenderer renderer, PlayerInput input)
		{
			_window = window;
			_renderer = renderer;
			_input = input;

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

			_input.BindKey(SFML.Window.Keyboard.Key.F, SwapPlayerWithBot);
			_renderer.Camera.SetFollowTarget(_mainPlayer);
		}

		private void SpawnUserUI()
		{
			_scoreText = _uiFactory.CreateScoreText(_mainPlayer.Mass.ToString());

			_countText = _uiFactory.CreateCountText($"Players: {_alivedPlayersCount}");

			_mainPlayer.OnAteFood += UpdateScore;

			OnPlayerDied += OnPlayerDead;
		}


		public override void UpdateInput()
		{
					
		}

		public override void Update(float deltaTime)
		{
			UpdatePlayerCamera();		

			CheckFoodColissions();

			CheckPlayerColissions();
			
			CheckForGameRestart();
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

					if (TryEatPlayer(player1, player2))
					{
						continue;
					}
				}
			}
		}


		private void UpdatePlayerCamera()
		{
			_renderer.Zoom(_mainPlayer.ZoomFactor);
		}

		private bool TryEatPlayer(Player player1, Player player2)
		{
			if (player1.TryEat(player2))
			{
				EatPlayer(player2);
				return true;
			}
			else if (player2.TryEat(player1))
			{
				EatPlayer(player1);
				return true;
			}

			return false;
		}
		
		private void EatPlayer(Player player)
		{
			player.SetActive(false);
			_alivedPlayersCount--;
			OnPlayerDied?.Invoke(_alivedPlayersCount);
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

			OnPlayerDead(_alivedPlayersCount);
		}

		private void OnPlayerDead(int playersCount)
		{
			string text = $"Players: {playersCount}";

			_countText.ChangeText(text);
		}

		private void UpdateScore(float mass)
		{
			string text = $"Mass: {MathF.Round(mass, 0)}";

			_scoreText.ChangeText(text);
		}




		//Trash

		private void SwapPlayerWithBot()
		{
			Player bot = null;

			foreach (var player in _players)
			{
				if (player != _mainPlayer && player.IsActive)
				{
					bot = player;
					break;
				}
			}

			if (bot == null)
			{
				return;
			}

			var mainPlayerMovement = _mainPlayer.GetComponent<PlayerMovementComponent>();
			var botMovement = bot.GetComponent<PlayerMovementComponent>();

			mainPlayerMovement.ChangePlayerMode(true);
			botMovement.ChangePlayerMode(false);

			_mainPlayer = bot;

			_renderer.Camera.SetFollowTarget(_mainPlayer);
		}
	}
}