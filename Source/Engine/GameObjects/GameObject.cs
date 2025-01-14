using SFML.Graphics;
using SFML.System;

namespace Source.Engine.GameObjects
{
	public interface IInpputHandler
	{
		public void UpdateInput();
	}

	public interface IUpdatable
	{
		public void Update(float deltaTime);
	}

	public interface IDisposable
	{
		public bool IsDisposed { get; }

		public event Action<GameObject> OnDisposed;

		public void Dispose();
	}

	public abstract class GameObject : IDisposable, IUpdatable, Drawable
	{
		public bool IsDisposed { get; private set; } = false;

		public event Action<GameObject> OnDisposed;

		public bool IsActive { get; private set; } = true;

		public virtual Vector2f Position { get; private set; }

		public Vector2f InitialPosition { get; }

		public GameObject() { }

		public GameObject(Vector2f initialPosition)
		{
			InitialPosition = initialPosition;
			Position = initialPosition;
		}

		public void Dispose()
		{
			if (IsDisposed)
			{
				return;
			}

			IsDisposed = true;

			OnDisposed?.Invoke(this);
		}

		public void SetActive(bool isActive)
		{
			IsActive = isActive;
		}

		public void SetPosition(Vector2f position)
		{
			Position = position;
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
