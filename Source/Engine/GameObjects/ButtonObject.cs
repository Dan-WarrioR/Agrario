using SFML.Graphics;
using SFML.System;
using SFML.Window;
using Source.Engine.Input;
using Source.Engine.Tools;

namespace Source.Engine.GameObjects
{
	public class ButtonObject : GameObject, IUIElement
	{
		private static PlayerInput PlayerInput => _playerInput ??= Dependency.Get<PlayerInput>();
		private static PlayerInput _playerInput;

		private static readonly Color DefaultTextColor = Color.White;
		private static readonly Color DefaultBackgroundColor = new(50, 50, 50);
		private static readonly Font DefaultFont = new(FontPath);

		private const string FontPath = @"C:\Windows\Fonts\Arial.ttf";
		private const uint CharacterSize = 24;

		private RectangleShape _background;
		private Text? _text;
		private Sprite? _icon;

		public Vector2f Position => _background.Position;
		public Vector2f Size => _background.Size;

		public event Action? OnClicked;

		public void Initialize(Vector2f size, Vector2f initialPosition, Texture? icon = null, string? text = null)
		{
			Initialize(initialPosition);

			_background = new RectangleShape(size)
			{
				FillColor = DefaultBackgroundColor,
				Position = initialPosition
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
					Position = new Vector2f(
						_background.Position.X + (_background.Size.X - CharacterSize * text.Length) / 2,
						_background.Position.Y + (_background.Size.Y - CharacterSize) / 2
					)
				};
			}

			PlayerInput.OnMousePressed += OnMousePressed;
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

		private bool IsMouseOver(Vector2i mousePos)
		{
			return _background.GetGlobalBounds().Contains(mousePos);
		}

		private void OnMousePressed(Mouse.Button button, Vector2i mousePosition)
		{
			if (!IsMouseOver(mousePosition))
			{
				return;
			}

			OnClicked?.Invoke();
		}
	}
}