﻿using SFML.Graphics;
using SFML.System;
using Source.Engine;
using Source.Engine.GameObjects;

namespace Source.Game.Units
{
    public class TextObject : GameObject, IUIElement
	{
        private static readonly Color DefaultTextColor = Color.White;
        private static readonly Font DefaultFont = new(FontPath);

        private const string FontPath = @"C:\Windows\Fonts\Arial.ttf";
        private const uint CharacterSize = 24;

        public Vector2f Position => _text.Position;

        private Text _text;

        public void Initialize(string text, Vector2f initialPosition)
        {
            Initialize(initialPosition);

			_text = new(text, DefaultFont, CharacterSize)
			{
				FillColor = DefaultTextColor,
				Position = initialPosition,
			};
		}

        public void ChangeText(string text)
        {
			_text.DisplayedString = text;
		}

        public override void Draw(RenderTarget target, RenderStates states)
        {
            _text.Draw(target, states);
        }
    }
}
