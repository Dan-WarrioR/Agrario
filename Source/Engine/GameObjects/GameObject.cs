using SFML.Graphics;
using SFML.System;

namespace Source.Engine.GameObjects
{
	public interface IInpputHandler
	{
		public void UpdateInput();
	}

	public abstract class GameObject : Drawable
	{
		public bool IsActive { get; private set; } = true;

		public Vector2f InitialPosition { get; }

		public GameObject() { }

		public GameObject(Vector2f initialPosition)
		{
			InitialPosition = initialPosition;
		}

		public void SetActive(bool isActive)
		{
			IsActive = isActive;
		}

		public virtual void Start()
		{

		}

		public virtual void Update(float deltaTime)
		{
			
		}

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
