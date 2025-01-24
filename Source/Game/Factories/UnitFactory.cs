using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Game.Configs;
using Source.Engine;
using Source.Game.Units.Components;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

		private static readonly Color BotFillColor = new(46, 204, 113);
		private static readonly Color PlayerFillColor = new(52, 152, 219);

		public UnitFactory(GameLoop gameLoop, BaseRenderer renderer) : base(gameLoop, renderer)
        {

		}

		#region Food

		public Food SpawnFood()
		{
			var food = Instantiate<Food>();

			food.Initialize(FoodRadius, GetRandomPosition());

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
			var player = Instantiate<Player>();

			player.Initialize(PlayerFillColor, 50, GetRandomPosition());
			player.AddComponent<PlayerControllerComponent>();

            return player;
		}

		public Player SpawnBot()
        {
			var bot = Instantiate<Player>();

			bot.Initialize(BotFillColor, MinPlayerRadius, GetRandomPosition());
			bot.AddComponent<AiMovementComponent>();

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
    }
}
