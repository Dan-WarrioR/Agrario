using Source.Engine.Systems;
using SFML.System;
using SFML.Window;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Tools;
using Source.Game.Configs;

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
        private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private AudioManager AudioManager => _audioManager ??= Dependency.Get<AudioManager>();
		private AgarioGame _game;
		private GameCamera _camera;
        private PlayerInput _playerInput;
		private AudioManager _audioManager;

        public Player Player { get; private set; }

		private string _restartSound;
		private string _swapSound;
		private string _eatSound;

		public override void SetTarget(GameObject target)
		{
			if (Player != null)
			{
				Player.OnAteFood -= OnAteFood;
			}

			base.SetTarget(target);

			var player = (Player)base.Target;

			Player = player;

			if (Player != null)
			{
				Player.OnAteFood += OnAteFood;
			}
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

			_restartSound = AudioConfig.RestarGameSound;
			_swapSound = AudioConfig.SwapSound;
			_eatSound = AudioConfig.EatSound;

			PlayerInput.BindKey(Keyboard.Key.Escape, Game.StopGame);
            PlayerInput.BindKey(Keyboard.Key.R, RestartGame);
            PlayerInput.BindKey(Keyboard.Key.F, SwapPlayers);
        }

		private void SwapPlayers()
		{
			List<Player> bots = new();

			var mainPlayer = Game.PlayerController.Player;

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

			Game.PlayerController.SetTarget(bot);

			Camera.SetFollowTarget(bot);

			AudioManager.PlayOnced(_swapSound);
		}

		private void RestartGame()
		{
			AudioManager.PlayOnced(_restartSound);
			Game.RestartGame();
		}

		private void OnAteFood(float value)
		{
			AudioManager.PlayOnced(_eatSound);
		}
	}
}