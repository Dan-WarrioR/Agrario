namespace Source.Game.Configs
{
	public static class PlayerConfig
	{
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
