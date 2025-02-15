using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Game.Configs;
using Source.Engine;
using Source.Game.Units.Controllers;
using Source.Engine.GameObjects.Components;
using Source.Engine.Tools;
using Source.Engine.Configs;
using Source.Engine.Systems.Animation;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private static SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private static SFMLRenderer _renderer;

		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;
		private const float FrameDuration = 0.1f;

		private string _monsterSpritePath;
		private string _slimeSpritePath;
		private string _skullIdleSpritePath;
		private string _skullAggresiveSpritePath;
		private string _rockIdleSpritePath;
		private string _rockRunSpritePath;
		private FloatRect _bounds;

		public UnitFactory() : base(Renderer)
        {
			SetupConfigsValues();
		}

		private void SetupConfigsValues()
		{
			_monsterSpritePath = PlayerConfig.MonsterSpritePath;
			_slimeSpritePath = PlayerConfig.SlimeSpritePath;
			_skullIdleSpritePath = PlayerConfig.SkullIdleSpritePath;
			_skullAggresiveSpritePath = PlayerConfig.SkullAggresiveSpritePath;
			_rockIdleSpritePath = PlayerConfig.RockIdleSpritePath;
			_rockRunSpritePath = PlayerConfig.RockRunSpritePath;
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

			var idle = new AnimationState("Idle", AnimationLoader.GetTextures(_skullIdleSpritePath), FrameDuration);
			var run = new AnimationState("Run", AnimationLoader.GetTextures(_skullAggresiveSpritePath), FrameDuration);			

			animator.AddAnimation(idle);
			animator.AddAnimation(run);
			animator.AddTransition(new AnimationTransition("Idle", "Run", player.IsMoving));
			animator.AddTransition(new AnimationTransition("Run", "Idle", () => !player.IsMoving()));

			animator.Play("Run");

			return playerConroller;
		}

		public Player SpawnBot()
        {
			var bot = Instantiate<Player>();
			var botConroller = Instantiate<BotController>();

			bot.Initialize(botConroller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());

			var animator = bot.AddComponent<Animator>();

			var idle = new AnimationState("Idle", AnimationLoader.GetTextures(_rockIdleSpritePath), FrameDuration);
			var run = new AnimationState("Run", AnimationLoader.GetTextures(_rockRunSpritePath), FrameDuration);

			animator.AddAnimation(idle);
			animator.AddAnimation(run);
			animator.AddTransition(new AnimationTransition("Idle", "Run", () => !bot.IsMoving()));
			animator.AddTransition(new AnimationTransition("Run", "Idle", () => !bot.IsMoving()));

			animator.Play("Idle");

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
			return new Color(255, 255, 255, 255);

			byte r = CustomRandom.RangeByte(0, 255);
			byte g = CustomRandom.RangeByte(0, 255);
			byte b = CustomRandom.RangeByte(0, 255);

			return new Color(r, g, b, 255);
		}
    }
}
