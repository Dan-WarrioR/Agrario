using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace Source.Engine.Configs
{
    public static class WindowConfig
    {
        public const string AppName = "Game";

        public static readonly Vector2f WindowSize = new(1920, 1080);

        public static readonly Styles Style = Styles.Fullscreen;

        public static readonly VideoMode VideoMode = VideoMode.DesktopMode;

        public static readonly FloatRect Bounds = new(0, 0, WindowSize.X, WindowSize.Y);

        public static readonly Color BackgroundColor = Color.Black;
    }
}