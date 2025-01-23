using SFML.Window;

namespace Source.Engine
{
	public class KeyBind
	{
		public event Action OnKeyPressed;

		private Keyboard.Key _key;

		private bool _wasPressed;
		private bool _isPressed;

		public KeyBind(Keyboard.Key key)
		{
			_key = key;
		}

		public void Update()
		{
			_wasPressed = _isPressed;
			_isPressed = Keyboard.IsKeyPressed(_key);

			if (_isPressed)
			{
				Invoke();
			}
		}

		public void Invoke()
		{
			OnKeyPressed?.Invoke();
		}

		public void Add(Action action)
		{
			OnKeyPressed += action;
		}

		public void Remove(Action action)
		{
			OnKeyPressed -= action;
		}
	}

	public class Input
	{
		private readonly Window _window;

		private readonly Dictionary<Keyboard.Key, KeyBind> _keyBindings;

		public Input(Window window)
		{
			_window = window;
			_keyBindings = new();
		}

		public void UpdateInput()
		{
			_window.DispatchEvents();

			foreach (var keyBind in _keyBindings.Values)
			{
				keyBind.Update();
			}
		}

		public void BindKey(Keyboard.Key key, Action action)
		{
			if (!_keyBindings.ContainsKey(key))
			{
				_keyBindings[key] = new(key);
			}

			_keyBindings[key].Add(action);
		}

		public void UnbindKey(Keyboard.Key key, Action action)
		{
			if (_keyBindings.TryGetValue(key, out var keyBind))
			{
				keyBind.Remove(action);
			}
		}
	}
}