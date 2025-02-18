using Source.Game.Factories;
using Source.Engine;
using Source.Game.Units;
using Source.Game.Configs;
using Source.Engine.GameObjects;
using Source.Engine.Tools;
using Source.Game.Units.Controllers;
using Source.Engine.Systems;
using Source.Game.Features.Audio;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private GameLoop GameLoop => _gameLoop ??= Dependency.Get<GameLoop>();
		private GameLoop _gameLoop;
		private SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private SFMLRenderer _renderer;

		private AudioManager _audioManager;
		private UnitFactory _unitFactory;
		private UIFactory _uiFactory;

		private TextObject _scoreText;
		private TextObject _countText;

		public PlayerController PlayerController;
		public IReadOnlyList<Player> Players => _players;

		private List<Food> _foods;
		private List<Player> _players;

		private event Action<int> OnPlayerDied;

		private bool _isEndGame = false;

		private int _alivedPlayersCount = 0;
		private int _playersCount;
		private int _foodCount;
		private string _musicSound;
		

		public override void Initialize()
		{
			Dependency.Register(this);

			_unitFactory = new();
			_uiFactory = new();
			new GameSoundEventHandler();

			SetupConfigValues();

			_audioManager = Dependency.Get<AudioManager>();
			
			_audioManager.SetVolume(_musicSound, 20f);
			_audioManager.PlayLooped(_musicSound);

			SpawnTerrain();
			SpawnFood();
			SpawnPlayers();	
			SpawnUserUI();
		}

		private void SetupConfigValues()
		{
			_playersCount = GameConfig.PlayersCount;
			_foodCount = GameConfig.FoodCount;
			_musicSound = AudioConfig.MusicSound;
		}

		private void SpawnTerrain()
		{
			_uiFactory.CreateTerrain();
		}

		private void SpawnFood()
		{
			_foods = new(_foodCount);

			for (int i = 0; i < _foodCount; i++)
			{
				var food = _unitFactory.SpawnFood();
				_foods.Add(food);
			}
		}

		private void SpawnPlayers()
		{
			_players = new(_playersCount);

			SpawnBots();
			SpawnMainPlayer();

			_alivedPlayersCount = _players.Count;
		}

		private void SpawnBots()
		{
			for (int i = 0; i < _playersCount - 1; i++)
			{
				var bot = _unitFactory.SpawnBot();

				_players.Add(bot);
			}
		}

		private void SpawnMainPlayer()
		{
			PlayerController = _unitFactory.SpawnPlayer();

			_players.Add(PlayerController.Player);

			Renderer.Camera.SetFollowTarget(PlayerController.Player);
		}

		private void SpawnUserUI()
		{
			_scoreText = _uiFactory.CreateScoreText(PlayerController.Player.Mass.ToString());

			_countText = _uiFactory.CreateCountText($"Players: {_alivedPlayersCount}");

			PlayerController.Player.OnAteFood += UpdateScore;

			OnPlayerDied += OnPlayerDead;
		}



		public override void OnUpdate(float deltaTime)
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
			_audioManager.StopAllSounds();
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
			if (!PlayerController.IsActive || _alivedPlayersCount > 1)
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
			Renderer.Zoom(PlayerController.Player.ZoomFactor);
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