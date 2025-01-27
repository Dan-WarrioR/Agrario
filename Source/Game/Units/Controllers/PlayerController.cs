using SFML.System;
using SFML.Window;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Tools;

namespace Source.Game.Units.Controllers
{
    public class PlayerController : BaseController
    {
        private static Vector2f NormalPlayerScale = new(1, 1);
		private static Vector2f MirroredPlayerScale = new(-1, 1);

		private static List<(Keyboard.Key Key, Vector2f Delta)> _keyMap = new()
        {
            new(Keyboard.Key.W, new(0, -1)),
            new(Keyboard.Key.S, new(0, 1)),
            new(Keyboard.Key.A, new(-1, 0)),
            new(Keyboard.Key.D, new(1, 0)),
        };

        private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
        private PlayerInput _playerInput;

        private Player _target;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			_target = (Player)Target;
		}

		public override void Start()
        {
            base.Start();

            foreach (var binding in _keyMap)
            {
                PlayerInput.BindKey(binding.Key,
                    onPressed: () => Delta += binding.Delta,
                    onReleased: () => Delta -= binding.Delta);
            }
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

			_target.SetDelta(Delta);
        }
    }
}