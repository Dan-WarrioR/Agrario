using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Engine.Configs;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Systems.GameFSM;
using Source.Engine.Tools;
using Source.Game.Factories;

namespace Source.Game.GameStates
{
	public class MainMenuState : BaseGameState
	{
		private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private PlayerInput _playerInput;

		private UIFactory _uiFactory;

		private List<ButtonObject> _buttons = new();

		public override void Enter()
		{
			_uiFactory = new();

			var WindowSize = WindowConfig.WindowSize;

			var startButton = CreateButton(new(200, 50), new((WindowSize.X - 200) / 2, 250), text: "Почати гру");
			var exitButton = CreateButton(new(200, 50), new((WindowSize.X - 200) / 2, 320), text: "Exit");

			CreateButton(new(50, 50), new((WindowSize.X - 300) / 2, 150), text: "<");
			CreateButton(new(50, 50), new((WindowSize.X + 200) / 2, 150), text: ">");

			//ShapeObejct is not a UI element. Fix it later
			//_skinPreview = new ShapeObject(new RectangleShape(new(100, 100)) { FillColor = Color.Blue });
			//_skinPreview.SetPosition(new((screenWidth - 100) / 2, 150));

			startButton.OnClicked += OnStartGameButtonClicked;
			exitButton.OnClicked += OnExitButtonClicked;

			PlayerInput.BindKey(Keyboard.Key.Escape, StopGame);
		}	

		public override void Update(float deltaTime)
		{
			
		}

		public override void Exit()
		{
			foreach (var button in _buttons)
			{
				button.Destroy();
			}

			PlayerInput.RemoveBind(Keyboard.Key.Escape);
		}

		private void OnExitButtonClicked()
		{
			StopGame();
		}

		private void StopGame()
		{
			var game = Dependency.Get<AgarioGame>();
			game.StopGame();
		}

		private void OnStartGameButtonClicked()
		{
			StateMachine.SetState<AgarioGameState>();
		}

		private ButtonObject CreateButton(Vector2f size, Vector2f initialPosition, Texture? icon = null, string? text = null)
		{
			var button = _uiFactory.CreateButton(size, initialPosition, icon, text);

			_buttons.Add(button);

			return button;
		}
	}
}