using Agrario.Source.Engine.GameObjects;
using SFML.System;
using Source.Engine;
using Source.Game.Configs;

namespace Source.Game.Factories
{
    public class UIFactory : ObjectFactory
	{
		private static readonly Vector2f ScoreTextPosition = new(WindowConfig.Bounds.Width - 200, WindowConfig.Bounds.Top + 20);
		private static readonly Vector2f PlayerCountTextPosition = new(WindowConfig.Bounds.Left + 50, WindowConfig.Bounds.Top + 20);

		private readonly GameLoop _gameLoop;
		private readonly BaseRenderer _renderer;

		public UIFactory(GameLoop gameLoop, BaseRenderer renderer) : base(gameLoop, renderer)
		{
			_gameLoop = gameLoop;
			_renderer = renderer;		
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
	}
}