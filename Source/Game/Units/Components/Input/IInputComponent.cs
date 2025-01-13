using SFML.System;

namespace Source.Game.Units.Components.Input
{
	public interface IInputComponent
	{
		public Vector2f Delta { get; }

		public void UpdateInput();

		public void Update(float deltaTime);
	}
}