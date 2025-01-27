using SFML.System;

namespace Source.Game.Configs
{
	public static class PlayerConfig
	{
		//Sprites

		public const string SlimeSpritePath = "Characters\\Slime";
		public const string EyeSpritePath = "Characters\\Eye";
		public const string MonsterSpritePath = "Characters\\Monster";

		//Scale
		public static Vector2f NormalPlayerScale = new(1, 1);
		public static Vector2f MirroredPlayerScale = new(-1, 1);

		//Speed
		public const float BaseSpeed = 200f;
		public const float MinSpeed = 100f;
		public const float SpeedReductionCoefficient = 20f;

		//Growth
		public const float GrowthBase = 2f;
		public const float GrowthDecayRate = 0.5f;
		public const float MinGrowthFactor = 1f;
		public const float MassMultiplier = 1f;
	}
}
