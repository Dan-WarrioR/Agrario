using SFML.Graphics;
using SFML.System;

namespace Source.Engine.Configs
{
    public static class WindowConfig
    {
        public static Vector2f WindowSize = new(1920, 1080);

        public static readonly FloatRect Bounds = new(0, 0, WindowSize.X, WindowSize.Y);
        public static Color BackgroundColor = Color.Black;
	}
}