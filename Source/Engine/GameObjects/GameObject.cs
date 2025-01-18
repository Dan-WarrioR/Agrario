using SFML.Graphics;
using SFML.System;

namespace Source.Engine.GameObjects
{
	public interface IInpputHandler
	{
		public void UpdateInput();
	}

	public interface IDisposable
	{
		public bool IsDisposed { get; }

		public event Action<GameObject> OnDisposed;

		public void Dispose();
	}

	public abstract class GameObject : IDisposable, Drawable
	{
		public bool IsDisposed { get; private set; } = false;

		public event Action<GameObject> OnDisposed;

		public bool IsActive { get; private set; } = true;

		public Vector2f InitialPosition { get; }

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

		public virtual void Start()
		{

		}

		public virtual void Update(float deltaTime)
		{
			
		}

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
