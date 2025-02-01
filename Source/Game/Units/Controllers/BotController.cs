using Source.Engine;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Game.Units.Controllers
{
    public class BotController : BaseController
	{
		private const int MinMovementDelay = 0;
		private const int MaxMovementDelay = 5;

		private Player _target;

		private float _movementDelay = 0f;
		private float _aiMovementTime = 0f;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			_target = (Player)Target;
		}

		public override void Start()
		{
			base.Start();

			_movementDelay = CustomRandom.Range(MinMovementDelay, MaxMovementDelay);
			SetRandomDelta();
		}

		public override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			_aiMovementTime += deltaTime;

			if (_aiMovementTime > _movementDelay)
			{
				_movementDelay = CustomRandom.Range(MinMovementDelay, MaxMovementDelay);
				SetRandomDelta();
			}		
		}

		private void SetRandomDelta()
		{
			_aiMovementTime = 0f;

			float angle = CustomRandom.NextSingle() * MathF.PI * 2;

			Delta = new(MathF.Cos(angle), MathF.Sin(angle));
		}
	}
}