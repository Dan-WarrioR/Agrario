using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Game.Configs;
using Source.Engine;
using Source.Game.Units.Controllers;
using Source.Engine.GameObjects.Components;
using Source.Engine.Tools;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

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
			var playerConroller = Instantiate<PlayerController>();

			player.Initialize(playerConroller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());
			var animator = player.AddComponent<Animator>();
			animator.Initialize(PlayerConfig.MonsterSpritePath, 0.1f);

            return player;
		}

		public Player SpawnBot()
        {
			var bot = Instantiate<Player>();
			var botConroller = Instantiate<BotController>();

			bot.Initialize(botConroller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());
			var animator = bot.AddComponent<Animator>();
			animator.Initialize(PlayerConfig.SlimeSpritePath, 0.1f);

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

		private Color GetRandomColor()
		{
			byte r = CustomRandom.RangeByte(0, 255);
			byte g = CustomRandom.RangeByte(0, 255);
			byte b = CustomRandom.RangeByte(0, 255);

			return new Color(r, g, b);
		}
    }
}
