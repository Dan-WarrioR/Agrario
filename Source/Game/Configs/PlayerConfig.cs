using SFML.System;
using Source.Engine.Configs;

namespace Source.Game.Configs
{
	public static class PlayerConfig
	{
		static PlayerConfig()
		{
			ConfigLoader.LoadConfig(typeof(PlayerConfig));
		}

		//Sprites

		public static string SlimeSpritePath = "Characters\\Slime";
		public static string EyeSpritePath = "Characters\\Eye";
		public static string MonsterSpritePath = "Characters\\Monster";

		//Scale
		public static Vector2f NormalPlayerScale = new(1, 1);
		public static Vector2f MirroredPlayerScale = new(-1, 1);

		//Speed
		public static float BaseSpeed = 200f;
		public static float MinSpeed = 100f;
		public static float SpeedReductionCoefficient = 20f;

		//Growth
		public static float GrowthBase = 2f;
		public static float GrowthDecayRate = 0.5f;
		public static float MinGrowthFactor = 1f;
		public static float MassMultiplier = 1f;
	}
}