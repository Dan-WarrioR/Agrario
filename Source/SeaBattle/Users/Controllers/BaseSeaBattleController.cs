using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.SeaBattle.LevelMap;

namespace Source.SeaBattle.Users.Controllers
{
	public class BaseSeaBattleController : BaseController
	{
		public Player Player { get; private set; }

		protected Vector2i? ShootPosition;

		public override void SetTarget(GameObject target)
		{
			base.SetTarget(target);

			var player = (Player)Target;

			Player = player;
		}

		public virtual BombState TryShoot()
		{
			return BombState.None;
		}
	}
}