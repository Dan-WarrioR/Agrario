﻿using SFML.Window;
using Source.Engine.Tools;

namespace Agrario.Source.Engine.Input
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

        private readonly List<KeyBind> _toUnbindBindings = new();
        private readonly List<KeyBind> _toBindBindings = new();

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

            UnbindBindings();
            BindNewBindings();
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

        public bool TryGetBind(Keyboard.Key key, KeyBind keyBind)
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
    }
}