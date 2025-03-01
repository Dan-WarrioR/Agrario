using SFML.System;
using Source.Engine;
using Source.SeaBattle.Users;
using Source.SeaBattle.Users.Controllers;

namespace Source.SeaBattle.Factories
{
	public class UnitFactory : ObjectFactory
	{
		public Player SpawnPlayer(Vector2f position)
		{
			var (unit, controller) = CreateUnit<PlayerController>(position);

			return unit;
		}

		public Player SpawnEnemy(Vector2f position)
		{
			var (unit, controller) = CreateUnit<PlayerController>(position);

			return unit;
		}

		private (Player unit, TController controller) CreateUnit<TController>(Vector2f position) where TController : BaseSeaBattleController, new()
		{
			var unit = Instantiate<Player>(position);
			var controller = Instantiate<TController>();

			controller.SetTarget(unit);
			unit.SetController(controller);

			return (unit, controller);
		}
	}
}