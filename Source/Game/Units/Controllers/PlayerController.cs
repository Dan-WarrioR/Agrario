﻿using SFML.System;
using SFML.Window;
using Source.Engine.Systems;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Input;
using Source.Engine.Rendering;
using Source.Engine.Tools;
using Source.Game.GameStates;

namespace Source.Game.Units.Controllers
{
    public class PlayerController : BaseController, IPauseHandler
    {
		private static List<(Keyboard.Key Key, Vector2f Delta)> _movementBindingsMap = new()
        {
            new(Keyboard.Key.W, new(0, -1)),
            new(Keyboard.Key.S, new(0, 1)),
            new(Keyboard.Key.A, new(-1, 0)),
            new(Keyboard.Key.D, new(1, 0)),
        };
		
        private EventBus EventBus => _eventBus ??= Dependency.Get<EventBus>();
		private AgarioGameState Game => _game ??= Dependency.Get<AgarioGameState>();
		private GameCamera Camera => _camera ??= Dependency.Get<GameCamera>();
        private PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private PauseManager PauseManager => _pauseManager ??= Dependency.Get<PauseManager>();
		private AgarioGameState _game;
		private GameCamera _camera;
        private PlayerInput _playerInput;
		private PauseManager _pauseManager;
		private EventBus _eventBus;

        public Player Player { get; private set; }
	
		public override void OnStart()
        {
			PauseManager.Register(this);

            foreach (var binding in _movementBindingsMap)
            {
                PlayerInput.BindKey(binding.Key,
                    onPressed: () => Delta += binding.Delta,
                    onReleased: () => Delta -= binding.Delta);
            }

			PlayerInput.BindKey(Keyboard.Key.Escape, StopGame);
            PlayerInput.BindKey(Keyboard.Key.R, RestartGame);
            PlayerInput.BindKey(Keyboard.Key.F, SwapPlayers);
            PlayerInput.BindKey(Keyboard.Key.P, PauseGame);
        }

		public override void SetTarget(GameObject target)
		{
			if (Player != null)
			{
				Player.OnAteFood -= OnAteFood;
			}

			base.SetTarget(target);

			var player = (Player)Target;

			Player = player;

			if (Player != null)
			{
				Player.OnAteFood += OnAteFood;
			}
		}
		
		private void SwapPlayers()
		{
			EventBus.Invoke("OnSwapPlayers");
		}

		private void RestartGame()
		{
			EventBus.Invoke("OnGameRestart");			
		}

		private void StopGame()
		{
			EventBus.Invoke("OnStopGame");
		}

		private void OnAteFood(float value)
		{
			EventBus.Invoke("OnPlayerAteFood");
		}

		private void PauseGame()
		{
			PauseManager.Switch();
		}

		void IPauseHandler.SetPaused(bool isPaused)
		{
			if (isPaused)
			{
				foreach (var binding in _movementBindingsMap)
				{
					PlayerInput.RemoveBind(binding.Key);
				}

				Delta = new(0, 0);
			}
            else
            {
				foreach (var binding in _movementBindingsMap)
				{
					PlayerInput.BindKey(binding.Key,
						onPressed: () => Delta += binding.Delta,
						onReleased: () => Delta -= binding.Delta);
				}
			}
        }
	}
}