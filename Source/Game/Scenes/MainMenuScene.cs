using SFML.System;
using SFML.Graphics;
using SFML.Window;
using Source.Engine.GameObjects.Components;
using Source.Engine.GameObjects.UI;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Systems.SceneSystem;
using Source.Engine.Systems.Tools.Animations;
using Source.Engine.Tools;
using Source.Engine.Configs;
using Source.Engine.Systems;
using Source.Engine.Systems.Animation;
using Source.Game.Data.Saves;
using Source.Game.Factories;
using Source.Game.Features.SaveSystem;
using Source.Game.Configs;

namespace Source.Game.Scenes
{
	public class MainMenuScene : Scene
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;

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

		public override void Load()
		{
			_uiFactory = new UIFactory();

			_saveService = new();
			_saveData = SaveService.Load<PlayerSaveData>();

			_windowSize = WindowConfig.WindowSize;

			CreateMenu();
			SpawnPlayerSkinAvatar();
			
			PlayerInput.BindKey(Keyboard.Key.Escape, OnExitButtonClicked);
		}

		public override void Unload()
		{
			_startButton.OnClicked -= OnStartGameButton;
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

		public override void Update(float deltaTime) { }

		private void CreateMenu()
		{
			_startButton = CreateButton(new(200, 50), new((_windowSize.X - 200) / 2, 250), text: "Почати гру");
			_exitButton = CreateButton(new(200, 50), new((_windowSize.X - 200) / 2, 320), text: "Exit");
			_leftArrowButton = CreateButton(new(50, 50), new((_windowSize.X - 300) / 2, 125), text: "<");
			_rightArrowButton = CreateButton(new(50, 50), new((_windowSize.X + 200) / 2, 125), text: ">");

			_startButton.OnClicked += OnStartGameButton;
			_exitButton.OnClicked += OnExitButtonClicked;
			_leftArrowButton.OnClicked += OnLeftArrowClicked;
			_rightArrowButton.OnClicked += OnRightArrowClicked;
		}

		private void OnExitButtonClicked()
		{
			EventBus.Invoke("OnStopGame");
		}

		private void OnStartGameButton()
		{
			EventBus.Invoke("OnGameStart");
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