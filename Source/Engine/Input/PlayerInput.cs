﻿using SFML.System;
using SFML.Window;
using Source.Engine.Tools;

namespace Source.Engine.Input
{
    public class PlayerInput
    {
        public Vector2i MousePosition => Mouse.GetPosition(_window);

        public event Action<Mouse.Button, Vector2i> OnMousePressed;
        public event Action<Mouse.Button, Vector2i> OnMouseReleased;
        public event Action<Vector2i> OnMouseMoved;

        private readonly Window _window;

        private readonly Dictionary<Keyboard.Key, KeyBind> _keyBindings;

        private readonly List<KeyBind> _toUnbindBindings = new();
        private readonly List<KeyBind> _toBindBindings = new();

        public PlayerInput(Window window)
        {
            _window = window;
            _keyBindings = new();
            Dependency.Register(this);

            _window.MouseButtonPressed += OnMouseButtonPressed;
            _window.MouseButtonReleased += OnMouseButtonReleased;
            _window.MouseMoved += OnMouseDeviceMoved;
		}

		~PlayerInput()
        {
			_window.MouseButtonPressed -= OnMouseButtonPressed;
			_window.MouseButtonReleased -= OnMouseButtonReleased;
			_window.MouseMoved -= OnMouseDeviceMoved;
		}

		#region Bindings

		public void UpdateInputStates()
        {
            _window.DispatchEvents();

            foreach (var keyBind in _keyBindings.Values)
            {
                keyBind.CheckBindStates();
            }		

			UnbindBindings();
            BindNewBindings();
        }

        public void InvokeBindings()
        {
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

            _toBindBindings.Add(keyBind);
        }

        public void BindKey(Keyboard.Key key, Action onPressed = null, Action onHeld = null, Action onReleased = null)
        {
            var keyBind = new KeyBind(key);
            _toBindBindings.Add(keyBind);

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
            if (!_keyBindings.ContainsKey(key))
            {
                return;
            }

            _toUnbindBindings.Add(_keyBindings[key]);
        }

        public bool TryGetBind(Keyboard.Key key, out KeyBind keyBind)
        {
            return _keyBindings.TryGetValue(key, out keyBind);
        }

        private void BindNewBindings()
        {
            foreach (var item in _toBindBindings)
            {
                if (!_keyBindings.ContainsKey(item.Key))
                {
                    _keyBindings.Add(item.Key, item);
                }
            }

            _toBindBindings.Clear();
        }

        private void UnbindBindings()
        {
            foreach (var item in _toUnbindBindings)
            {
                _keyBindings.Remove(item.Key);
            }

            _toUnbindBindings.Clear();
        }

		#endregion



		#region Mouse

		private void OnMouseButtonReleased(object? sender, MouseButtonEventArgs e)
        {
            OnMousePressed?.Invoke(e.Button, new(e.X, e.Y));
        }

        private void OnMouseButtonPressed(object? sender, MouseButtonEventArgs e)
        {
			OnMouseReleased?.Invoke(e.Button, new(e.X, e.Y));
		}

        private void OnMouseDeviceMoved(object? sender, MouseMoveEventArgs e)
        {
            OnMouseMoved?.Invoke(new(e.X, e.Y));
        }

		#endregion
	}
}