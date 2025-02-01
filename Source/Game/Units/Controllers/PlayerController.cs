using SFML.System;
using SFML.Window;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Tools;

namespace Source.Game.Units.Controllers
{
    public class PlayerController : BaseController
    {
        private static Vector2f NormalPlayerScale = new(1, 1);
		private static Vector2f MirroredPlayerScale = new(-1, 1);

		private static List<(Keyboard.Key Key, Vector2f Delta)> _movementBindingsMap = new()
        {
            new(Keyboard.Key.W, new(0, -1)),
            new(Keyboard.Key.S, new(0, 1)),
            new(Keyboard.Key.A, new(-1, 0)),
            new(Keyboard.Key.D, new(1, 0)),
        };

		private AgarioGame Game => _game ??= Dependency.Get<AgarioGame>();
		private GameCamera Camera => _camera ??= Dependency.Get<GameCamera>();
		private AgarioGame _game;
		private GameCamera _camera;

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

            foreach (var binding in _movementBindingsMap)
            {
                PlayerInput.BindKey(binding.Key,
                    onPressed: () => Delta += binding.Delta,
                    onReleased: () => Delta -= binding.Delta);
            }

            PlayerInput.BindKey(Keyboard.Key.Escape, Game.StopGame);
            PlayerInput.BindKey(Keyboard.Key.R, Game.RestartGame);
            PlayerInput.BindKey(Keyboard.Key.F, SwapPlayers);
        }

		private void SwapPlayers()
		{
			List<Player> bots = new();

			var mainPlayer = Game.MainPlayer;

			foreach (var player in Game.Players)
			{
				if (player != mainPlayer && player.IsActive)
				{
					bots.Add(player);
				}
			}

			if (bots.Count <= 0)
			{
				return;
			}

			int randomIndex = CustomRandom.Range(0, bots.Count);
			var bot = bots[randomIndex];

			mainPlayer.SwapControllers(bot);

			mainPlayer = bot;

			Camera.SetFollowTarget(mainPlayer);
		}
	}
}