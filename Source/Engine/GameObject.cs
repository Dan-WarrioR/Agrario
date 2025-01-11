﻿using SFML.Graphics;
using SFML.System;

namespace Source.Engine
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

		public virtual Vector2f Position { get; }

		protected Vector2f InitialPosition { get; }

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

		public virtual void Update(float deltaTime)
		{
			
		}

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
