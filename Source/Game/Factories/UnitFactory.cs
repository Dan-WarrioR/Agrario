using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects.Components;
using Source.Engine.Tools;
using Source.Engine.Configs;
using Source.Engine.Systems.Animation;
using Source.Engine.Tools.ProjectUtilities;
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.GameObjects;
using Source.Game.Configs;
using Source.Game.Features.SaveSystem;
using Source.Game.Data.Saves;
using Source.Game.Units.Controllers;
using Source.Game.Units;

namespace Source.Game.Factories
{
    public class UnitFactory : ObjectFactory
    {
		private PlayerSaveData PlayerSaveData => SaveService.Load<PlayerSaveData>();

		private SaveService SaveService => _saveService ??= Dependency.Get<SaveService>();
		private SaveService _saveService;

		private const float MinPlayerRadius = 20f;
		private const float FoodRadius = 5f;

		private FloatRect _bounds;

		private AnimationGraph _enemyGraph;
		private AnimationGraph _playerGraph;

		public UnitFactory()
        {
	        Dependency.Register(this);
	        
			SetupConfigsValues();

			//_playerGraph = BuidlPlayerAnimationGraph();
			//_enemyGraph = BuidlBotAnimationGraph();
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
        
        private void SetupAnimator(GameObject unit, AnimationGraphBuilder data)
        {
	        var animator = unit.AddComponent<Animator>();

	        animator.Setup(data);
        }

		private AnimationGraphBuilder BuidlPlayerAnimationGraph()
		{
			var skin = GetCurrentPlayerSkin();

			return new AnimationGraphBuilder()
				.AddState("Idle", skin.idleSprites, 0.1f)
				.AddState("Run", skin.runSprites, 0.1f)
				.SetInitialState("Idle")
				.AddTransition("Idle", "Run")
				.AddBoolConditionTo("Idle", "IsMoving", true)
				.AddTransition("Run", "Idle")
				.AddBoolConditionTo("Run", "IsMoving", false);
		}

		private AnimationGraphBuilder BuidlBotAnimationGraph()
		{
			return new AnimationGraphBuilder()
				.AddState("Idle", TextureLoader.GetSpritesheetTextures(PlayerConfig.RockIdleSpritePath), 0.1f)
				.AddState("Run", TextureLoader.GetSpritesheetTextures(PlayerConfig.RockRunSpritePath), 0.1f)
				.SetInitialState("Idle")
				.AddTransition("Idle", "Run")
				.AddBoolConditionTo("Idle", "IsMoving", true)
				.AddTransition("Run", "Idle")
				.AddBoolConditionTo("Run", "IsMoving", false);
		}

		#endregion

		private (List<Texture> idleSprites, List<Texture> runSprites) GetCurrentPlayerSkin()
		{
			(string idle, string run) = PlayerSaveData.SkinIndex switch
			{
				0 => (PlayerConfig.SkullIdleSpritePath, PlayerConfig.SkullAggresiveSpritePath),
				1 => (PlayerConfig.RockIdleSpritePath, PlayerConfig.RockRunSpritePath),
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
