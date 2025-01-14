using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Game.Units.Components.Input;
using Source.Game.Configs;
using Source.Engine;
using Source.Engine.Tools;

namespace Source.Game.Factories
{
    public class UnitFactory
    {
		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

		private static readonly Color BotFillColor = new(150, 150, 150);
		private static readonly Color PlayerFillColor = Color.Yellow;

        public ObjectPool<Food> FoodPool;
        public ObjectPool<Player> BotsPool;

        private Player _mainPlayer;

		private GameLoop _gameLoop;
		private BaseRenderer _renderer;

		public UnitFactory(GameLoop gameLoop, BaseRenderer renderer)
        {
            _gameLoop = gameLoop;
            _renderer = renderer;

            FoodPool = new(0, 500, SpawnFood, RespawnFood);
            BotsPool = new(0, 50, SpawnBot, RespawnBot);
		}

		#region Food

		private Food SpawnFood()
		{
			var food = new Food(FoodRadius, GetRandomPosition());

			RegisterObject(food);

			return food;
		}

		private void RespawnFood(Food food)
		{
			food.SetActive(true);
			food.SetPosition(GetRandomPosition());
		}

		#endregion



		#region Players

		public Player SpawnPlayer()
        {
			_mainPlayer = new Player(new KeyBoardInput(), PlayerFillColor, MinPlayerRadius, GetRandomPosition());
            
			RegisterObject(_mainPlayer);

            return _mainPlayer;
		}

		private Player SpawnBot()
        {
			var player = new Player(new RandomDirectionInput(), BotFillColor, MinPlayerRadius, GetRandomPosition());

			RegisterObject(player);

			return player;
		}

        private void RespawnBot(Player player)
        {
			player.SetActive(false);
			player.SetPosition(GetRandomPosition());
        }

		#endregion


		private Vector2f GetRandomPosition()
        {
            var bounds = WindowConfig.Bounds;

            float x = Random.Shared.Next((int)bounds.Left, (int)(bounds.Left + bounds.Width));
            float y = Random.Shared.Next((int)bounds.Top, (int)(bounds.Top + bounds.Height));

            return new(x, y);
        }

        private void RegisterObject(GameObject gameObject)
        {
            _gameLoop.Register(gameObject);
            _renderer.AddGameElement(gameObject);

            gameObject.OnDisposed += UnregisterObject;
        }

        private void UnregisterObject(GameObject gameObject)
        {
            gameObject.OnDisposed -= UnregisterObject;

            _renderer.RemoveGameElement(gameObject);
            _gameLoop.UnRegister(gameObject);
        }
    }
}
