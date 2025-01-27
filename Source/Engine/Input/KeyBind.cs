using SFML.Window;

namespace Source.Engine.Input
{
	public class KeyBind
	{
		public event Action OnKeyPressed;
		public event Action OnKeyHeld;
		public event Action OnKeyReleased;

		public Keyboard.Key Key;

		private bool _wasPressed;
		private bool _isPressed;

		public KeyBind(Keyboard.Key key)
		{
			Key = key;
		}

		public void Update()
		{
			_wasPressed = _isPressed;
			_isPressed = Keyboard.IsKeyPressed(Key);

			if (_isPressed)
			{
				if (!_wasPressed)
				{
					OnKeyPressed?.Invoke();
				}

				OnKeyHeld?.Invoke();
			}
			else
			{
				if (_wasPressed)
				{
					OnKeyReleased?.Invoke();
				}
			}
		}
	}
}
