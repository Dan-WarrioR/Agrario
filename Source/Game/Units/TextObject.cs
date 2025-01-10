using SFML.Graphics;
using SFML.System;
using Source.Engine;

namespace Source.Game.Units
{
	public class TextObject : GameObject
	{
		private static readonly Color TextColor = Color.White;

		private const string FontPath = @"C:\Windows\Fonts\Arial.ttf";
		private const uint CharacterSize = 24;

		public override Vector2f Position => _text.Position;

		private Text _text;

		public TextObject(Vector2f initialPosition) : base(initialPosition)
		{
			var font = new Font(FontPath);

			_text = new("0", font, CharacterSize)
			{
				FillColor = TextColor,
				Position = initialPosition,
			};
		}

		public void OnScoreChanged(int playerScore)
		{
			_text.DisplayedString = playerScore.ToString();
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			_text.Draw(target, states);
		}
	}
}
