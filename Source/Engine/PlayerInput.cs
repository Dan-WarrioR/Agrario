using SFML.Window;
using Source.Engine.Tools;

namespace Source.Engine
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

	public class PlayerInput
	{
		private readonly Window _window;

		private readonly Dictionary<Keyboard.Key, KeyBind> _keyBindings;

		public PlayerInput(Window window)
		{
			_window = window;
			_keyBindings = new();
			Dependency.Register(this);
		}

		public void UpdateInput()
		{
			_window.DispatchEvents();

			foreach (var keyBind in _keyBindings.Values)
			{
				keyBind.Update();
			}
		}

		public void BindKey(KeyBind keyBind)
		{
			if (_keyBindings.ContainsValue(keyBind))
			{
				return;
			}

			_keyBindings.Add(keyBind.Key, keyBind);
		}

		public void BindKey(Keyboard.Key key, Action onPressed = null, Action onHeld = null, Action onReleased = null)
		{
			if (!_keyBindings.TryGetValue(key, out var keyBind))
			{
				keyBind = new KeyBind(key);
				_keyBindings[key] = keyBind;
			}

			if (onPressed != null)
			{
				keyBind.OnKeyPressed += onPressed;
			}

			if (onHeld != null)
			{
				keyBind.OnKeyHeld += onHeld;
			}

			if (onReleased != null)
			{
				keyBind.OnKeyReleased += onReleased;
			}
		}

		public void RemoveBind(Keyboard.Key key)
		{
			_keyBindings.Remove(key);
		}

		public bool TryGetBind(Keyboard.Key key, KeyBind keyBind)
		{
			return _keyBindings.TryGetValue(key, out keyBind);
		}
	}
}