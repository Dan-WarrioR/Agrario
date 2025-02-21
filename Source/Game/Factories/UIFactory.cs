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


		public UIFactory() : base(Renderer)
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

			textObject.Initialize(text, ScoreTextPosition);

			return textObject;
		}

		public TextObject CreateCountText(string text)
		{
			var textObject = Instantiate<TextObject>();

			textObject.Initialize(text, PlayerCountTextPosition);

			return textObject;
		}

		public TextObject CreateText(Vector2f position, string text)
		{
			var textObject = Instantiate<TextObject>();

			textObject.Initialize(text, position);

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