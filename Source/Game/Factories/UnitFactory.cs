using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;
using Source.Game.Units.Components.Input;
using Source.Game.Configs;
using Source.Engine;

namespace Source.Game.Factories
{
    public class UnitFactory
    {
		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

		private static readonly Color BotFillColor = new(46, 204, 113);
		private static readonly Color PlayerFillColor = new(52, 152, 219);

		private GameLoop _gameLoop;
		private BaseRenderer _renderer;

		public UnitFactory(GameLoop gameLoop, BaseRenderer renderer)
        {
            _gameLoop = gameLoop;
            _renderer = renderer;
		}

		#region Food

		public Food SpawnFood()
		{
			var food = new Food(FoodRadius, GetRandomPosition());

			RegisterObject(food);

			return food;
		}

		public void RespawnFood(Food food)
		{
			food.SetActive(true);
			food.SetPosition(GetRandomPosition());
		}

		#endregion



		#region Players

		public Player SpawnPlayer()
        {
			var player = new Player(new KeyBoardInput(), PlayerFillColor, 50, GetRandomPosition());
            
			RegisterObject(player);

            return player;
		}

		public Player SpawnBot()
        {
			var bot = new Player(new RandomDirectionInput(), BotFillColor, MinPlayerRadius, GetRandomPosition());

			RegisterObject(bot);

			return bot;
		}

        public void RespawnPlayer(Player player)
        {
			player.Reset();
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
