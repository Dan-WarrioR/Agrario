using SFML.Graphics;
using SFML.System;

namespace Source.Engine
{
	public interface IUpdatable
	{
		public void Update(float deltaTime);
	}

	public abstract class GameObject : IUpdatable, Drawable
	{
		public virtual Vector2f Position { get; }

		protected Vector2f InitialPosition { get; }

		public GameObject(Vector2f initialPosition)
		{
			InitialPosition = initialPosition;
			Position = initialPosition;

			GameLoop.Register(this);
		}

		~GameObject()
		{
			GameLoop.UnRegister(this);
		}

		public virtual void Update(float deltaTime)
		{
			
		}

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
