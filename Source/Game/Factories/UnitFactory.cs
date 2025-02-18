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
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.Tools.ProjectUtilities;
using Source.Game.Data.Animations;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private static SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private static SFMLRenderer _renderer;
		
		private static TextureLoader TextureLoader => _textureLoader ??= Dependency.Get<TextureLoader>();
		private static TextureLoader _textureLoader;

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

		public (Player unit, TController controller) CreateUnit<TController>() where TController : BaseController, new()
		{
			var unit = Instantiate<Player>();
			var controller = Instantiate<TController>();
			
			unit.Initialize(controller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());
			
			return (unit, controller);
		}
		
		public PlayerController SpawnPlayer()
		{
			var player = CreateUnit<PlayerController>();

			SetupAnimator(player.unit, new PlayerAnimationData(player.unit));

			return player.controller;
		}

		public Player SpawnBot()
        {
			var bot = CreateUnit<BotController>();
			
			SetupAnimator(bot.unit, new EnemyAnimationData(bot.unit));

			return bot.unit;
		}

        public void RespawnPlayer(Player player)
        {
			player.Reset();
			player.SetPosition(GetRandomPosition());
        }
        
        private void SetupAnimator<T>(Player unit, T data) where T : AnimationData
        {
	        var animator = unit.AddComponent<Animator>();

	        animator.Setup(data);
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
