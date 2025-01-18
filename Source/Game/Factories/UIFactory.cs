using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;
using Source.Game.Configs;
using Source.Game.Units;

namespace Source.Game.Factories
{
	public class UIFactory
	{
		private static readonly Vector2f ScoreTextPosition = new(WindowConfig.Bounds.Width - 200, WindowConfig.Bounds.Top + 20);
		private static readonly Vector2f PlayerCountTextPosition = new(WindowConfig.Bounds.Left + 50, WindowConfig.Bounds.Top + 20);

		private readonly GameLoop _gameLoop;
		private readonly BaseRenderer _renderer;

		public UIFactory(GameLoop gameLoop, BaseRenderer renderer)
		{
			_gameLoop = gameLoop;
			_renderer = renderer;		
		}

		public TextObject CreateScoreText(string text)
		{
			var textObject = new TextObject(text, ScoreTextPosition);

			RegisterText(textObject);

			return textObject;
		}

		public TextObject CreateCountText(string text)
		{
			var textObject = new TextObject(text, PlayerCountTextPosition);

			RegisterText(textObject);

			return textObject;
		}

		private void RegisterText(GameObject textObject)
		{
			_gameLoop.Register(textObject);
			_renderer.AddUIElement(textObject);
		}
	}
}