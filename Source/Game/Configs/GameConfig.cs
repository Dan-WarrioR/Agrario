using Source.Engine.Configs;

namespace Source.Game.Configs
{
	public static class GameConfig
	{
		static GameConfig()
		{
			ConfigLoader.LoadConfig(typeof(GameConfig));
		}

		public static int PlayersCount = 100;
		public static int FoodCount = 1000;
	}
}
