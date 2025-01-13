﻿using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects;

namespace Source.Game.Units
{
    public class TextObject : GameObject
    {
        private static readonly Color DefaultTextColor = Color.White;
        private static readonly Font DefaultFont = new(FontPath);

        private const string FontPath = @"C:\Windows\Fonts\Arial.ttf";
        private const uint CharacterSize = 24;

        public override Vector2f Position => _text.Position;

        private Text _text;

        public TextObject(string text, Vector2f initialPosition) : base(initialPosition)
        {
            _text = new(text, DefaultFont, CharacterSize)
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
