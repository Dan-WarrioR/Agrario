using Source.Game.Units;
using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Game.Units.Controllers;
using Source.Engine.GameObjects.Components;
using Source.Engine.Tools;
using Source.Engine.Configs;
using Source.Engine.Systems.Animation;
using Source.Engine.Tools.ProjectUtilities;
using Source.Engine.Systems.Tools.Animations;
using Source.Game.Configs;
using Source.Engine.GameObjects;
using Source.Game.Features.SaveSystem;
using Source.Game.Data.Saves;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private SaveService SaveService => _saveService ??= Dependency.Get<SaveService>();
		private SaveService _saveService;

		private TextureLoader TextureLoader => _textureLoader ??= Dependency.Get<TextureLoader>();
		private TextureLoader _textureLoader;
		private static SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private static SFMLRenderer _renderer;

		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

		private FloatRect _bounds;

		private AnimationGraph _enemyGraph;
		private AnimationGraph _playerGraph;

		private PlayerSaveData _saveData;

		public UnitFactory() : base(Renderer)
        {
	        Dependency.Register(this);
	        
			SetupConfigsValues();

			//_playerGraph = BuidlPlayerAnimationGraph();
			//_enemyGraph = BuidlBotAnimationGraph();
			_saveData = SaveService.Load<PlayerSaveData>();
		}
		
		~UnitFactory()
		{
			Dependency.Unregister(this);
		}

		private void SetupConfigsValues()
		{
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

		public T SpawnCircle<T>(float radius, Vector2f position) where T : CircleObject, new()
		{
			var gameObject = Instantiate<T>();

			gameObject.Initialize(radius, position);

			return gameObject;
		}

		public (Player unit, TController controller) CreateUnit<TController>() where TController : BaseController, new()
		{
			var unit = Instantiate<Player>();
			var controller = Instantiate<TController>();
			
			unit.Initialize(controller, GetRandomColor(), MinPlayerRadius, GetRandomPosition());
			
			return (unit, controller);
		}
		
		public PlayerController SpawnPlayer()
		{
			var (unit, controller) = CreateUnit<PlayerController>();

			SetupAnimator(unit, BuidlPlayerAnimationGraph());

			return controller;
		}

		public Player SpawnBot()
        {
			var (unit, controller) = CreateUnit<BotController>();
			
			SetupAnimator(unit, BuidlBotAnimationGraph());

			return unit;
		}

        public void RespawnPlayer(Player player)
        {
			player.Reset();
			player.SetPosition(GetRandomPosition());
        }
        
        private void SetupAnimator<T>(GameObject unit, T data) where T : AnimationGraph
        {
	        var animator = unit.AddComponent<Animator>();

	        animator.Setup(data);
        }

		private AnimationGraph BuidlPlayerAnimationGraph()
		{
			var skin = GetCurrentPlayerSkin();

			return new AnimationGraphBuilder()
				.AddState("Idle", skin.idleSprites, 0.1f)
				.AddState("Run", skin.runSprites, 0.1f)
				.SetInitialState("Idle")
				.AddTransition("Idle", "Run")
				.AddBoolConditionTo("Idle", "IsMoving", true)
				.AddTransition("Run", "Idle")
				.AddBoolConditionTo("Run", "IsMoving", false)
				.Build();
		}

		private AnimationGraph BuidlBotAnimationGraph()
		{
			return new AnimationGraphBuilder()
				.AddState("Idle", TextureLoader.GetSpritesheetTextures(PlayerConfig.RockIdleSpritePath), 0.1f)
				.AddState("Run", TextureLoader.GetSpritesheetTextures(PlayerConfig.RockRunSpritePath), 0.1f)
				.SetInitialState("Idle")
				.AddTransition("Idle", "Run")
				.AddBoolConditionTo("Idle", "IsMoving", true)
				.AddTransition("Run", "Idle")
				.AddBoolConditionTo("Run", "IsMoving", false)
				.Build();
		}

		#endregion

		private (List<Texture> idleSprites, List<Texture> runSprites) GetCurrentPlayerSkin()
		{
			(string idle, string run) = _saveData.SkinIndex switch
			{
				0 => (PlayerConfig.SkullIdleSpritePath, PlayerConfig.SkullAggresiveSpritePath),
				1 => (PlayerConfig.RockIdleSpritePath, PlayerConfig.RockIdleSpritePath),
			};

			return (TextureLoader.GetSpritesheetTextures(idle), TextureLoader.GetSpritesheetTextures(run));
		}

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
