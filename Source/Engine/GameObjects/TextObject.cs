using SFML.Graphics;
using SFML.System;

namespace Source.Engine.GameObjects
{
    public class TextObject : GameObject
	{
		private static readonly Color DefaultTextColor = Color.Black;

		private const string FontPath = @"C:\Windows\Fonts\Arial.ttf";
		private const uint CharacterSize = 24;

		public override Vector2f Position => _text.Position;

		private Text _text;

		public TextObject(string text, Vector2f initialPosition) : base(initialPosition)
		{
			var font = new Font(FontPath);

			_text = new(text, font, CharacterSize)
			{
				FillColor = DefaultTextColor,
				Position = initialPosition,
			};
		}

		public TextObject(string text, Vector2f initialPosition, Color textColor) : this(text, initialPosition)
		{
			_text.FillColor = textColor;
		}

		public void OnScoreChanged(float playerMass)
		{
			_text.DisplayedString = playerMass.ToString();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			_text.Draw(target, states);
		}
	}
}
