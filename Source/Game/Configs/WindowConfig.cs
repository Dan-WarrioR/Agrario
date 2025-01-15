using SFML.Graphics;
using SFML.System;

namespace Source.Game.Configs
{
    public static class WindowConfig
    {
		public const string AppName = "Air Hockey";

		public static readonly Vector2f WindowSize = new(1920, 1080);

		public static readonly FloatRect Bounds = new(0, 0, WindowSize.X, WindowSize.Y);

		public static readonly Color BackgroundColor = Color.Black;
	}
}