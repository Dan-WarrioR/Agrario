using SFML.System;
using SFML.Window;
using Source.Engine.Input;
using Source.Engine.Systems;
using Source.Engine.Tools;
using Source.SeaBattle.LevelMap;
using Source.SeaBattle.Objects;

namespace Source.SeaBattle.Users.Controllers
{
	public class PlayerController : BaseSeaBattleController
	{
		private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private EventBus _eventBus;
		private GameManager Game => _game ??= Dependency.Get<GameManager>();
		private GameManager _game;
		private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private PlayerInput _playerInput;

		public override void OnStart()
		{
			PlayerInput.BindKey(Keyboard.Key.Escape, StopGame);
			PlayerInput.OnMousePressed += OnMousePressed;
		}

		public override BombState TryShoot()
		{
			if (ShootPosition.HasValue)
			{
				var state = Player.TryBombCell(ShootPosition.Value);

				ShootPosition = null;

				return state;
			}

			return BombState.None;
		}

		private void StopGame()
		{
			EventBus.Invoke("OnStopGame");
		}

		private void OnMousePressed(Mouse.Button button, Vector2i position)
		{
			if (button != Mouse.Button.Left)
			{
				return;
			}

			ShootPosition = position;
		}
	}
}