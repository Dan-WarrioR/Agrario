﻿using Source.Game.Factories;
using SFML.Graphics;
using Source.Engine;
using Source.Game.Units;
using Source.Game.Configs;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private GameLoop GameLoop => _gameLoop ??= Dependency.Get<GameLoop>();
		private GameLoop _gameLoop;
		private RenderWindow _window;
		private SFMLRenderer _renderer;

		private UnitFactory _unitFactory;
		private UIFactory _uiFactory;

		private TextObject _scoreText;
		private TextObject _countText;

		public Player MainPlayer;
		public IReadOnlyList<Player> Players => _players;

		private List<Food> _foods = new(GameConfig.FoodCount);
		private List<Player> _players = new(GameConfig.PlayersCount);

		private event Action<int> OnPlayerDied;

		private bool _isEndGame = false;

		private int _alivedPlayersCount = 0;

		public void Initialize(RenderWindow window, SFMLRenderer renderer)
		{
			Dependency.Register(this);

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
			MainPlayer = _unitFactory.SpawnPlayer();

			_players.Add(MainPlayer);

			_renderer.Camera.SetFollowTarget(MainPlayer);
		}

		private void SpawnUserUI()
		{
			_scoreText = _uiFactory.CreateScoreText(MainPlayer.Mass.ToString());

			_countText = _uiFactory.CreateCountText($"Players: {_alivedPlayersCount}");

			MainPlayer.OnAteFood += UpdateScore;

			OnPlayerDied += OnPlayerDead;
		}



		public override void Update(float deltaTime)
		{
			UpdatePlayerCamera();		

			CheckFoodColissions();

			CheckPlayerColissions();
			
			CheckForGameRestart();
		}

		public override bool IsEndGame()
		{
			return _isEndGame;
		}

		public void StopGame()
		{
			_isEndGame = true;
		}

		public void RestartGame()
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



		private void CheckForGameRestart()
		{
			if (!MainPlayer.IsActive || _alivedPlayersCount > 1)
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
					player.TryEat(food);
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
			_alivedPlayersCount--;
			OnPlayerDied?.Invoke(_alivedPlayersCount);
		}

		private void UpdatePlayerCamera()
		{
			_renderer.Zoom(MainPlayer.ZoomFactor);
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
	}
}