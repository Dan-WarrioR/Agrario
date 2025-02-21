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

namespace Source.Game.GameStates
{
    public class MainMenuState : BaseGameState
	{
		private SaveService SaveService => _saveService ??= Dependency.Get<SaveService>();
		private SaveService _saveService;

		private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private PlayerInput _playerInput;

		private UIFactory _uiFactory;

		private List<ButtonObject> _buttons = new();

		private ButtonObject _startButton;
		private ButtonObject _exitButton;
		private ButtonObject _leftArrowButton;
		private ButtonObject _rightArrowButton;

		private PlayerSaveData _saveData;

		private TextObject _skinText;

		public override void Enter()
		{
			_uiFactory = new();
			_saveService = new();

			var WindowSize = WindowConfig.WindowSize;

			_startButton = CreateButton(new(200, 50), new((WindowSize.X - 200) / 2, 250), text: "Почати гру");
			_exitButton = CreateButton(new(200, 50), new((WindowSize.X - 200) / 2, 320), text: "Exit");
			_leftArrowButton = CreateButton(new(50, 50), new((WindowSize.X - 300) / 2, 150), text: "<");
			_rightArrowButton = CreateButton(new(50, 50), new((WindowSize.X + 200) / 2, 150), text: ">");
			_skinText = _uiFactory.CreateText(new(WindowSize.X / 2, 160));

			//ShapeObejct is not a UI element. Fix it later
			//_skinPreview = new ShapeObject(new RectangleShape(new(100, 100)) { FillColor = Color.Blue });
			//_skinPreview.SetPosition(new((screenWidth - 100) / 2, 150));

			_saveData = SaveService.Load<PlayerSaveData>();

			_skinText.SetText(_saveData.SkinIndex);

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

			foreach (var button in _buttons)
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
			_skinText.SetText(_saveData.SkinIndex);
		}
		

		private void StopGame()
		{
			var game = Dependency.Get<AgarioGame>();
			game.StopGame();
		}

		private ButtonObject CreateButton(Vector2f size, Vector2f initialPosition, Texture? icon = null, string? text = null)
		{
			var button = _uiFactory.CreateButton(size, initialPosition, icon, text);

			_buttons.Add(button);

			return button;
		}
	}
}