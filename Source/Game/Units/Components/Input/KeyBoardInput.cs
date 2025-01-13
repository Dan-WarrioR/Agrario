using SFML.System;
using SFML.Window;

namespace Source.Game.Units.Components.Input
{
	public struct MovementKey
	{
		public Keyboard.Key Key { get; }
		public float DeltaX { get; }
		public float DeltaY { get; }

		public MovementKey(Keyboard.Key key, float deltaX, float deltaY)
		{
			Key = key;
			DeltaX = deltaX;
			DeltaY = deltaY;
		}

		public bool IsPressed()
		{
			return Keyboard.IsKeyPressed(Key);
		}
	}

	public class KeyBoardInput : IInputComponent
	{
		public Vector2f Delta { get; private set; }

		private List<MovementKey> _keyMap = new()
		{
			new(Keyboard.Key.W, 0, -1),
			new(Keyboard.Key.S, 0, 1),
			new(Keyboard.Key.A, -1, 0),
			new(Keyboard.Key.D, 1, 0),
		};

		public void UpdateInput()
		{
			float deltaX = 0;
			float deltaY = 0;

			foreach (var key in _keyMap)
			{
				if (key.IsPressed())
				{
					deltaX += key.DeltaX;
					deltaY += key.DeltaY;
				}
			}

			Delta = new(deltaX, deltaY);
		}

		public void Update(float deltaTime)
		{

		}
	}
}