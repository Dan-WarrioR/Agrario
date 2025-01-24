using SFML.Window;
using Source.Engine.Tools;

namespace Source.Engine
{
	public class KeyBind
	{
		public event Action OnKeyPressed;
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
				OnKeyPressed?.Invoke();
			}
			else
			{
				if (_wasPressed)
				{
					OnKeyReleased?.Invoke();
					_wasPressed = false;
				}
			}		
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