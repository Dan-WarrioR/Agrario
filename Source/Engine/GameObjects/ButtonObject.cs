using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Engine.Input;
using Source.Engine.Rendering;
using Source.Engine.Tools;

namespace Source.Engine.GameObjects
{
	public class ButtonObject : GameObject, IUIElement
	{
		private static PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private static PlayerInput _playerInput;

		private static readonly Color DefaultTextColor = Color.White;
		private static readonly Color DefaultBackgroundColor = new(50, 50, 50);
		private static readonly Color HoverBackgroundColor = new(80, 80, 80);
		private static readonly Font DefaultFont = new(FontPath);

		private const string FontPath = @"C:\\Windows\\Fonts\\Arial.ttf";
		private const uint CharacterSize = 24;

		private RectangleShape _background;
		private Text? _text;
		private Sprite? _icon;
		private bool _isHovered = false;
		
		public Vector2f Size => _background.Size;

		public event Action? OnClicked;

		public void Initialize(Vector2f size, Vector2f initialPosition, Texture? icon = null, string? text = null)
		{
			Initialize(initialPosition);

			_background = new RectangleShape(size)
			{
				FillColor = DefaultBackgroundColor,
				Position = initialPosition,
			};

			if (icon != null)
			{
				_text = null;
				_icon = new Sprite(icon)
				{
					Position = new Vector2f(
						_background.Position.X + (_background.Size.X - icon.Size.X) / 2,
						_background.Position.Y + (_background.Size.Y - icon.Size.Y) / 2
					)
				};
			}
			else if (text != null)
			{
				_icon = null;
				_text = new Text(text, DefaultFont, CharacterSize)
				{
					FillColor = DefaultTextColor,
				};

				CenterText();
			}

			PlayerInput.OnMousePressed += OnMousePressed;
			PlayerInput.OnMouseMoved += OnMouseMoved;
		}

		public override void Draw(RenderTarget target, RenderStates states)
		{
			target.Draw(_background, states);

			if (_text != null)
			{
				target.Draw(_text, states);
			}
			else if (_icon != null)
			{
				target.Draw(_icon, states);
			}
		}

		public void SetText(object text)
		{
			if (_text == null)
			{
				return;
			}

			_text.DisplayedString = text.ToString();
			CenterText();
		}

		private void CenterText()
		{
			if (_text == null)
			{
				return;
			}

			var textBounds = _text.GetLocalBounds();
			_text.Origin = new Vector2f(textBounds.Left + textBounds.Width / 2, textBounds.Top + textBounds.Height / 2);
			_text.Position = new Vector2f(
				_background.Position.X + _background.Size.X / 2,
				_background.Position.Y + _background.Size.Y / 2
			);
		}

		private bool IsMouseOver(Vector2i mousePos)
		{
			return _background.GetGlobalBounds().Contains(mousePos.X, mousePos.Y);
		}

		private void OnMousePressed(Mouse.Button button, Vector2i mousePosition)
		{
			if (!IsMouseOver(mousePosition))
			{
				return;
			}

			OnClicked?.Invoke();
		}

		private void OnMouseMoved(Vector2i mousePosition)
		{
			bool wasHovered = _isHovered;
			_isHovered = IsMouseOver(mousePosition);

			if (_isHovered && !wasHovered)
			{
				_background.FillColor = HoverBackgroundColor;
			}
			else if (!_isHovered && wasHovered)
			{
				_background.FillColor = DefaultBackgroundColor;
			}
		}
	}
}