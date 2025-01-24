using SFML.System;
using SFML.Window;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Game.Units.Components
{
	public class PlayerControllerComponent : BaseComponent
	{
		private List<(Keyboard.Key Key, Vector2f Delta)> _keyMap = new()
		{
			new(Keyboard.Key.W, new(0, -1)),
			new(Keyboard.Key.S, new(0, 1)),
			new(Keyboard.Key.A, new(-1, 0)),
			new(Keyboard.Key.D, new(1, 0)),
		};

		private Vector2f _delta;
		private Player _player;

		public override void Start()
		{
			_player = (Player)Owner;
			var input = Dependency.Get<PlayerInput>();

			foreach (var binding in _keyMap)
			{
				var keyBind = new KeyBind(binding.Key);

				keyBind.OnKeyPressed += () => _delta += binding.Delta;
				keyBind.OnKeyReleased += () => _delta -= binding.Delta;

				input.BindKey(keyBind);
			}
		}

		public override void Update(float deltaTime)
		{
			Vector2f positionDelta = _player.CurrentSpeed * deltaTime * _delta;

			_player.SetPosition(_player.Position + positionDelta);

			_delta.X = 0; 
			_delta.Y = 0;
		}
	}
}