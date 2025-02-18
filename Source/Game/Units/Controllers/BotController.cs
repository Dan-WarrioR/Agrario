using SFML.System;
using Source.Engine;
using Source.Engine.Systems;
using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;

namespace Source.Game.Units.Controllers
{
    public class BotController : BaseController, IPauseHandler
	{
		private const int MinMovementDelay = 0;
		private const int MaxMovementDelay = 5;
		private const int MinIdleDelay = 1;
		private const int MaxIdleDelay = 3;

		private const float IdleChance = 0.2f;

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;

		private Vector2f _cachedDelta;

		private bool _isPaused;

		public override void OnStart()
		{
			var pauseManager = Dependency.Get<PauseManager>();
			pauseManager.Register(this);
			_isPaused = pauseManager.IsPaused;
			
			_movementDelay = CustomRandom.Range(MinMovementDelay, MaxMovementDelay);
			DecideNextAction();
		}

		public override void OnUpdate(float deltaTime)
		{
			if (_isPaused)
			{
				return;
			}
			
			_aiMovementTime += deltaTime;

			if (_aiMovementTime > _movementDelay)
			{
				_movementDelay = CustomRandom.Range(MinMovementDelay, MaxMovementDelay);
				DecideNextAction();
			}
		}

		private void DecideNextAction()
		{
			_aiMovementTime = 0f;

			if (CustomRandom.NextSingle() < IdleChance)
			{
				_movementDelay = CustomRandom.Range(MinIdleDelay, MaxIdleDelay);
				Delta = new(0, 0);
			}
			else
			{
				SetRandomDelta();
			}
		}

		private void SetRandomDelta()
		{
			float angle = CustomRandom.NextSingle() * MathF.PI * 2;
			Delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}

		void IPauseHandler.SetPaused(bool isPaused)
		{
			_isPaused = isPaused;
			
			if (_isPaused)
			{
				_cachedDelta = Delta;
				Delta = new(0, 0);
				return;
			}
			
			Delta = _cachedDelta;
		}
	}
}