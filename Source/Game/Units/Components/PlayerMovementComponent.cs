using SFML.System;
using SFML.Window;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;
using Source.Game.Configs;
using Random = Source.Engine.Tools.Random;

namespace Source.Game.Units.Components
{
	public class PlayerMovementComponent : BaseComponent
	{
		private const int MinMovementDelay = 2;
		private const int MaxMovementDelay = 10;

		private static List<(Keyboard.Key Key, Vector2f Delta)> _keyMap = new()
		{
			new(Keyboard.Key.W, new(0, -1)),
			new(Keyboard.Key.S, new(0, 1)),
			new(Keyboard.Key.A, new(-1, 0)),
			new(Keyboard.Key.D, new(1, 0)),
		};

		private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private PlayerInput _playerInput;

		public bool IsAi { get; private set; }

		public Vector2f CurrentPosition { get; private set; }	

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;

		private Vector2f _delta;
		private Player _player;

		public override void Start()
		{
			_player = (Player)Owner;

			if (IsAi)
			{
				SetRandomDelta();			

				_movementDelay = Random.Range(MinMovementDelay, MaxMovementDelay);
			}
			else
			{
				BindPlayerKeys();
			}	
		}

		public override void Update(float deltaTime)
		{
			Vector2f positionDelta = _player.CurrentSpeed * deltaTime * _delta;

			CurrentPosition = _player.Position + positionDelta;

			if (IsAi)
			{
				_aiMovementTime += deltaTime;

				if (_aiMovementTime > _movementDelay)
				{
					SetRandomDelta();
				}
			}
			else
			{
				_delta.X = 0;
				_delta.Y = 0;
			}

			ClampPosition();

			_player.SetPosition(CurrentPosition);
		}

		public void ChangePlayerMode(bool isAi)
		{
			if (!IsAi && isAi)
			{
				UnbindPlayerKeys();
				SetRandomDelta();
			}
			else if (IsAi && !isAi)
			{
				BindPlayerKeys();
			}

			IsAi = isAi;
		}

		private void BindPlayerKeys()
		{
			foreach (var binding in _keyMap)
			{
				PlayerInput.BindKey(binding.Key, 
					onHeld: () => _delta += binding.Delta, 
					onReleased: () => _delta -= binding.Delta);
			}
		}

		private void UnbindPlayerKeys()
		{
			foreach (var binding in _keyMap)
			{
				PlayerInput.RemoveBind(binding.Key);
			}
		}

		private void ClampPosition()
		{
			var bounds = WindowConfig.Bounds;

			float x = Math.Clamp(CurrentPosition.X, bounds.Left, bounds.Width);
			float y = Math.Clamp(CurrentPosition.Y, bounds.Top, bounds.Height);

			CurrentPosition = new(x, y);
		}

		private void SetRandomDelta()
		{
			_aiMovementTime = 0f;

			float angle = Random.NextSingle() * MathF.PI * 2;

			_delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}