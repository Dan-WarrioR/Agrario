using SFML.System;
using Source.Engine;
using Source.Game.Configs;
using Source.Game.Units;

namespace Source.Game.Factories
{
	public class UIFactory
	{
		private static readonly Vector2f ScoreTextPosition = new(WindowConfig.Bounds.Width - 200, WindowConfig.Bounds.Top + 20);

		private GameLoop _gameLoop;
		private BaseRenderer _renderer;

		public UIFactory(GameLoop gameLoop, BaseRenderer renderer)
		{
			_gameLoop = gameLoop;
			_renderer = renderer;		
		}

		public TextObject CreateText(string text)
		{
			var textObject = new TextObject(text, ScoreTextPosition);

			_renderer.AddUIElement(textObject);

			textObject.OnDisposed += _renderer.RemoveGameElement;

			return textObject;
		}
	}
}