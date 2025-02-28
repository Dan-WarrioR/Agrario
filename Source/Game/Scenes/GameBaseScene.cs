using Source.Engine.GameObjects;
using Source.Engine.Rendering;
using Source.Engine.Systems.SceneSystem;
using Source.Engine.Tools;
using Source.Engine.Systems;
using Source.Engine.Tools.ProjectUtilities;
using Source.Game.Factories;
using Source.Game.Units.Controllers;
using Source.Game.Units;
using Source.Game.Features.Audio;
using Source.Game.Configs;

namespace Source.Game.Scenes
{
	public class GameBaseScene : BaseScene
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

		private SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private SFMLRenderer _renderer;

		private GameCamera Camera => _camera ??= Dependency.Get<GameCamera>();
		private GameCamera _camera;

		private UIFactory UIFactory => _uiFactory ??= Dependency.Get<UIFactory>();
		private UIFactory _uiFactory;

		private UnitFactory UnitFactory => _unitFactory ??= Dependency.Get<UnitFactory>();
		private UnitFactory _unitFactory;

		private TextObject _scoreText;
		private TextObject _countText;

		public PlayerController PlayerController;
		public IReadOnlyList<Player> Players => _players;

		private List<Food> _foods;
		private List<Player> _players;

		private event Action<int> OnPlayerDied;

		private int _alivedPlayersCount = 0;
		private int _playersCount;
		private int _foodCount;

		private GameSoundManager _gameSoundManager;

		public override void Load()
		{
			_gameSoundManager = new();

			_gameSoundManager.PlayMainMenuMusic();

			SetupConfigValues();

			SpawnTerrain();
			SpawnFood();
			SpawnPlayers();
			SpawnUserUI();

			EventBus.Register("OnGameRestart", RestartGame);
			EventBus.Register("OnSwapPlayers", SwapPlayers);
		}

		public override void Unload()
		{
			EventBus.Unregister("OnGameRestart", RestartGame);
			EventBus.Unregister("OnSwapPlayers", SwapPlayers);
		}

		private void SetupConfigValues()
		{
			_playersCount = GameConfig.PlayersCount;
			_foodCount = GameConfig.FoodCount;
		}



		#region Spawn Entities

		private void SpawnTerrain()
		{
			UIFactory.CreateTerrain();
		}

		private void SpawnFood()
		{
			_foods = new(_foodCount);

			for (int i = 0; i < _foodCount; i++)
			{
				var food = UnitFactory.SpawnFood();
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
				var bot = UnitFactory.SpawnBot();

				_players.Add(bot);
			}
		}

		private void SpawnMainPlayer()
		{
			PlayerController = UnitFactory.SpawnPlayer();

			_players.Add(PlayerController.Player);

			Camera.BeginGameView();
			Renderer.Camera.SetFollowTarget(PlayerController.Player);
		}

		private void SpawnUserUI()
		{
			_scoreText = UIFactory.CreateScoreText(PlayerController.Player.Mass.ToString());

			_countText = UIFactory.CreateCountText($"Players: {_alivedPlayersCount}");

			PlayerController.Player.OnAteFood += UpdateScore;

			OnPlayerDied += OnPlayerDead;
		}

		#endregion



		public override void Update(float deltaTime)
		{
			UpdatePlayerCamera();

			CheckFoodColissions();

			CheckPlayerColissions();

			CheckForGameRestart();
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

		private void SwapPlayers()
		{
			List<Player> bots = new();

			var mainPlayer = PlayerController.Player;

			foreach (var player in Players)
			{
				if (player != mainPlayer && player.IsActive)
				{
					bots.Add(player);
				}
			}

			if (bots.Count <= 0)
			{
				return;
			}

			int randomIndex = CustomRandom.Range(0, bots.Count);
			var bot = bots[randomIndex];

			mainPlayer.SwapControllers(bot);

			PlayerController.SetTarget(bot);

			Camera.SetFollowTarget(bot);

			EventBus.Invoke("OnPlayerSwapped");
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

			_countText.SetText(text);
		}

		private void UpdateScore(float mass)
		{
			string text = $"Mass: {MathF.Round(mass, 0)}";

			_scoreText.SetText(text);
		}
	}
}