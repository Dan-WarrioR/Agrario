using Source.Game.Features.SaveSystem;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Engine.Configs;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Systems.GameFSM;
using Source.Engine.Tools;
using Source.Game.Data.Saves;
using Source.Game.Factories;
using Source.Engine.Systems.Tools.Animations;
using Source.Game.Configs;
using Source.Engine.GameObjects.Components;
using Source.Engine.GameObjects.UI;
using Source.Engine.Systems.Animation;

namespace Source.Game.GameStates
{
    public class MainMenuState : BaseGameState
	{
		private TextureLoader TextureLoader => _textureLoader ??= Dependency.Get<TextureLoader>();
		private TextureLoader _textureLoader;
		private SaveService SaveService => _saveService ??= Dependency.Get<SaveService>();
		private SaveService _saveService;
		private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private PlayerInput _playerInput;

		private List<GameObject> _toDestroyObjects = new();

		private UIFactory _uiFactory;

		private ButtonObject _startButton;
		private ButtonObject _exitButton;
		private ButtonObject _leftArrowButton;
		private ButtonObject _rightArrowButton;

		private PlayerSaveData _saveData;
		private Vector2f _windowSize;

		private UICircleObject _skinObject;
		private Animator _skinObjectAnimator;

		public override void Enter()
		{
			_uiFactory = new UIFactory();
			
			_saveService = new();
			_saveData = SaveService.Load<PlayerSaveData>();

			_windowSize = WindowConfig.WindowSize;

			_startButton = CreateButton(new(200, 50), new((_windowSize.X - 200) / 2, 250), text: "Почати гру");
			_exitButton = CreateButton(new(200, 50), new((_windowSize.X - 200) / 2, 320), text: "Exit");
			_leftArrowButton = CreateButton(new(50, 50), new((_windowSize.X - 300) / 2, 125), text: "<");
			_rightArrowButton = CreateButton(new(50, 50), new((_windowSize.X + 200) / 2, 125), text: ">");

			SpawnPlayerSkinAvatar();

			_startButton.OnClicked += OnStartGameButtonClicked;
			_exitButton.OnClicked += OnExitButtonClicked;
			_leftArrowButton.OnClicked += OnLeftArrowClicked;
			_rightArrowButton.OnClicked += OnRightArrowClicked;
			PlayerInput.BindKey(Keyboard.Key.Escape, StopGame);
		}	

		public override void Update(float deltaTime)
		{
			
		}

		public override void Exit()
		{
			_startButton.OnClicked -= OnStartGameButtonClicked;
			_exitButton.OnClicked -= OnExitButtonClicked;
			_leftArrowButton.OnClicked -= OnLeftArrowClicked;
			_rightArrowButton.OnClicked -= OnRightArrowClicked;

			PlayerInput.RemoveBind(Keyboard.Key.Escape);

			SaveService.Save(_saveData);

			foreach (var button in _toDestroyObjects)
			{
				button.Destroy();
			}		
		}

		private void OnExitButtonClicked()
		{
			StopGame();
		}

		private void OnStartGameButtonClicked()
		{
			StateMachine.SetState<AgarioGameState>();
		}

		private void OnRightArrowClicked()
		{
			_saveData.SkinIndex++;

			UpdateSkin();
		}

		private void OnLeftArrowClicked()
		{
			_saveData.SkinIndex--;
			
			UpdateSkin();
		}

		private void UpdateSkin()
		{
			_skinObjectAnimator.SetFrames("Idle", GetSkinTextures());
		}

		private void StopGame()
		{
			var game = Dependency.Get<AgarioGame>();
			game.StopGame();
		}

		private ButtonObject CreateButton(Vector2f size, Vector2f initialPosition, Texture? icon = null, string? text = null)
		{
			var button = _uiFactory.CreateButton(size, initialPosition, icon, text);

			_toDestroyObjects.Add(button);

			return button;
		}

		private void SpawnPlayerSkinAvatar()
		{
			var unitFactory = new UnitFactory();
			
			_skinObject = unitFactory.SpawnCircle<UICircleObject>(75, new(_windowSize.X / 2, 150));
			_skinObjectAnimator = _skinObject.AddComponent<Animator>();

			_skinObjectAnimator.Setup(new AnimationGraphBuilder()
				.AddState("Idle", GetSkinTextures(), 0.1f)
				.SetInitialState("Idle"));

			_toDestroyObjects.Add(_skinObject);
			UpdateSkin();
		}

		private List<Texture> GetSkinTextures()
		{
			string path = _saveData.SkinIndex switch
			{
				0 => PlayerConfig.SkullIdleSpritePath,
				1 => PlayerConfig.RockIdleSpritePath,
			};

			return TextureLoader.GetSpritesheetTextures(path);
		}
	}
}