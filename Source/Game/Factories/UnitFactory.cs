using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Game.Configs;
using Source.Engine;
using Source.Game.Units.Controllers;
using Source.Engine.GameObjects.Components;
using Source.Engine.Tools;
using Source.Engine.Configs;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private static SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private static SFMLRenderer _renderer;

		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

		private string _monsterSpritePath;
		private string _slimeSpritePath;
		private FloatRect _bounds;

		public UnitFactory() : base(Renderer)
        {
			SetupConfigsValues();
		}

		private void SetupConfigsValues()
		{
			_monsterSpritePath = PlayerConfig.MonsterSpritePath;
			_slimeSpritePath = PlayerConfig.SlimeSpritePath;
			_bounds = WindowConfig.Bounds;
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

		public PlayerController SpawnPlayer()
        {
			var player = Instantiate<Player>();
			var playerConroller = Instantiate<PlayerController>();

			player.Initialize(playerConroller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());
			var animator = player.AddComponent<Animator>();
			animator.Initialize(_monsterSpritePath, 0.1f);

            return playerConroller;
		}

		public Player SpawnBot()
        {
			var bot = Instantiate<Player>();
			var botConroller = Instantiate<BotController>();

			bot.Initialize(botConroller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());
			var animator = bot.AddComponent<Animator>();
			animator.Initialize(_slimeSpritePath, 0.1f);

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
            float x = Random.Shared.Next((int)_bounds.Left, (int)(_bounds.Left + _bounds.Width));
            float y = Random.Shared.Next((int)_bounds.Top, (int)(_bounds.Top + _bounds.Height));

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
