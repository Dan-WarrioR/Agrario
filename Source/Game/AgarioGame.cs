using Source.Game.Factories;
using SFML.Graphics;
using Source.Engine;
using Source.Game.Units;
using SFML.System;
using Source.Game.Configs;

namespace Source.Game
{
    public class AgarioGame : BaseGame
	{
		private const int PlayersCount = 2;
		private const int FoodCount = 500;

		private GameLoop GameLoop => GameLoop.Instance;
		private RenderWindow _window;
		private SFMLRenderer _renderer;

		private UnitFactory _unitFactory;
		private UIFactory _uiFactory;

		private TextObject _text;
		private Player _mainPlayer;

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

			SpawnUserUI();
		}

		private void SpawnBots()
		{
			for (int i = 0; i < PlayersCount - 1; i++)
			{
				_unitFactory.BotsPool.SpawnObject();
			}		
		}

		private void SpawnFood()
		{
			for (int i = 0; i < FoodCount; i++)
			{
				_unitFactory.FoodPool.SpawnObject();
			}
		}

		private void SpawnMainPlayer()
		{
			_mainPlayer = _unitFactory.SpawnPlayer();
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

			CheckPlayerColissions();

			CheckForGameRestart();
		}



		public bool IsEndGame()
		{
			return false;
		}



		private void CheckForGameRestart()
		{
			if (!_mainPlayer.IsActive || _unitFactory.BotsPool.Items.Count >= 1)
			{
				return;
			}

			RestartGame();
		}

		private void CheckFoodColissions()
		{
			foreach (var player in _unitFactory.BotsPool.Items)
			{
				foreach (var food in _unitFactory.FoodPool.Items)
				{
					if (player.TryEat(food))
					{
						food.Eat();
					}
				}		
			}

			//Fix it later

			foreach (var food in _unitFactory.FoodPool.Items)
			{
				if (_mainPlayer.TryEat(food))
				{
					food.Eat();
				}
			}
		}

		private void CheckPlayerColissions()
		{
			//Fix it later

			for (int i = 1; i < _unitFactory.BotsPool.Items.Count - 1; i++)
			{
				var bot1 = _unitFactory.BotsPool.Items[i - 1];
				var bot2 = _unitFactory.BotsPool.Items[i];

				if (bot2.IsActive && bot1.TryEat(bot2))
				{
					bot2.SetActive(false);
				}
				else if (bot1.IsActive && bot2.TryEat(bot1))
				{
					bot1.SetActive(false);
				}
			}

			//Fix it later

			foreach (var bot in _unitFactory.BotsPool.Items)
			{
				if (bot.IsActive && _mainPlayer.TryEat(bot))
				{
					bot.SetActive(false);
				}
				else if (_mainPlayer.IsActive && bot.TryEat(_mainPlayer))
				{
					_mainPlayer.SetActive(false);
				}
			}
		}

		private void RestartGame()
		{
			_unitFactory.FoodPool.RespawanAll();
			_unitFactory.BotsPool.RespawanAll();

			_mainPlayer.SetActive(true);
			_mainPlayer.SetPosition(GetRandomPosition());
		}

		private Vector2f GetRandomPosition()
		{
			var bounds = WindowConfig.Bounds;

			float x = Random.Shared.Next((int)bounds.Left, (int)(bounds.Left + bounds.Width));
			float y = Random.Shared.Next((int)bounds.Top, (int)(bounds.Top + bounds.Height));

			return new(x, y);
		}
	}
}