using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Source.Engine.Configs
{
    public class WindowConfig : BaseConfig
    {
		public override string Category => nameof(WindowConfig);

		public static string AppName { get; private set; } = "Game";
        public static Vector2f WindowSize { get; private set; } = new(1920, 1080);
        public static Styles Style { get; private set; } = Styles.Fullscreen;
        public static VideoMode VideoMode { get; private set; } = VideoMode.DesktopMode;
        public static FloatRect Bounds { get; private set; } = new(0, 0, WindowSize.X, WindowSize.Y);
        public static Color BackgroundColor { get; private set; } = Color.Black;

		public override Dictionary<string, object> GetConfigValues()
		{
			return new Dictionary<string, object>
			{
				{ nameof(AppName), AppName },
				{ nameof(WindowSize), $"{WindowSize.X},{WindowSize.Y}" },
			};
		}

		public override void Initialize(ConfigManager manager)
		{
			AppName = manager.GetValue(Category, nameof(AppName), AppName);
			WindowSize = manager.GetValue(Category, nameof(WindowSize), WindowSize);
		}
	}
}