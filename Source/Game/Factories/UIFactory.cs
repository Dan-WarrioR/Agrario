using Source.Engine.GameObjects;
using SFML.System;
using Source.Engine;
using Source.Engine.Configs;
using Source.Engine.Tools;
using Source.Game.UI.Terrain;
using SFML.Graphics;

namespace Source.Game.Factories
{
    public class UIFactory : ObjectFactory
	{
		private const string TerrainPath = "Terrain\\Grass.png";

		private static SFMLRenderer Renderer => _renderer ??= Dependency.Get<SFMLRenderer>();
		private static SFMLRenderer _renderer;

		private static readonly Vector2f ScoreTextPosition = new(WindowConfig.Bounds.Width - 200, WindowConfig.Bounds.Top + 20);
		private static readonly Vector2f PlayerCountTextPosition = new(WindowConfig.Bounds.Left + 50, WindowConfig.Bounds.Top + 20);


		public UIFactory()
		{
			Dependency.Register(this);
		}

		~UIFactory()
		{
			Dependency.Unregister(this);
		}

		public TextObject CreateScoreText(string text)
		{
			var textObject = Instantiate<TextObject>();

			textObject.Initialize(ScoreTextPosition, text);

			return textObject;
		}

		public TextObject CreateCountText(string text)
		{
			var textObject = Instantiate<TextObject>();

			textObject.Initialize(PlayerCountTextPosition, text);

			return textObject;
		}

		public TextObject CreateText(Vector2f position, string text = null)
		{
			var textObject = Instantiate<TextObject>();

			textObject.Initialize(position, text);

			return textObject;
		}

		public ButtonObject CreateButton(Vector2f size, Vector2f initialPosition, Texture? icon = null, string? text = null)
		{
			var buttonObject = Instantiate<ButtonObject>();

			buttonObject.Initialize(size, initialPosition, icon, text);

			return buttonObject;
		}

		public Terrain CreateTerrain()
		{
			var terrain = Instantiate<Terrain>();

			terrain.Initialize(TerrainPath);
			terrain.LoadTerrain();

			return terrain;
		}
	}
}