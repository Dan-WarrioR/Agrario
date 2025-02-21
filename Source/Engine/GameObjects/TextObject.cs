using SFML.Graphics;
using SFML.System;

namespace Source.Engine.GameObjects
{
    public class TextObject : GameObject, IUIElement
    {
        private static readonly Color DefaultTextColor = Color.White;
        private static readonly Font DefaultFont = new(FontPath);

        private const string FontPath = @"C:\Windows\Fonts\Arial.ttf";
        private const uint CharacterSize = 24;

        public Vector2f Position => _text.Position;

        private Text _text;

        public void Initialize(Vector2f initialPosition, string text = null)
        {
            base.Initialize(initialPosition);

            _text = new(text, DefaultFont, CharacterSize)
            {
                FillColor = DefaultTextColor,
                Position = initialPosition,
            };
        }

        public void SetText(object text)
        {
            _text.DisplayedString = text.ToString();
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _text.Draw(target, states);
        }
    }
}
