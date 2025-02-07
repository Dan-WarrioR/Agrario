using Source.Engine.Configs;

namespace Source.Game.Configs
{
	public class AudioConfig
	{
		static AudioConfig()
		{
			ConfigLoader.LoadConfig(typeof(AudioConfig));
		}

		public static string MusicSound = "Music";
		public static string EatSound = "Eat";
		public static string SwapSound = "Swap";
		public static string RestarGameSound = "RestartGame";
	}
}