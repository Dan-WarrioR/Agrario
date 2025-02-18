using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;
using Source.Engine.Tools.ProjectUtilities;

namespace Source.Game.Units.Controllers
{
    public class BotController : BaseController
	{
		private const int MinMovementDelay = 0;
		private const int MaxMovementDelay = 5;
		private const int MinIdleDelay = 1;
		private const int MaxIdleDelay = 3;

		private const float IdleChance = 0.2f;

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;
		private bool _isIdle = false;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);
		}

		public override void OnStart()
		{
			_movementDelay = CustomRandom.Range(MinMovementDelay, MaxMovementDelay);
			DecideNextAction();
		}

		public override void OnUpdate(float deltaTime)
		{
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
				_isIdle = true;
				_movementDelay = CustomRandom.Range(MinIdleDelay, MaxIdleDelay);
				Delta = new(0, 0);
			}
			else
			{
				_isIdle = false;
				SetRandomDelta();
			}
		}

		private void SetRandomDelta()
		{
			float angle = CustomRandom.NextSingle() * MathF.PI * 2;
			Delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}