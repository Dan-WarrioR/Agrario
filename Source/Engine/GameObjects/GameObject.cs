using SFML.Graphics;
using SFML.System;

namespace Source.Engine.GameObjects
{
	public interface IInpputHandler
	{
		public void UpdateInput();
	}

	public interface IActivable
	{
		public bool IsActive { get; }

		public void SetActive(bool isActive);
	}

	public abstract class GameObject : Drawable, IActivable
	{
		public bool IsActive { get; private set; } = true;

		public Vector2f InitialPosition { get; private set; }

		private List<BaseComponent> _components = new();

		public void Initialize(Vector2f initialPosition)
		{
			InitialPosition = initialPosition;
		}

		public void SetActive(bool isActive)
		{
			IsActive = isActive;
		}

		public T AddComponent<T>() where T : BaseComponent, new()
		{
			T component = new();

			component.SetOwner(this);
			_components.Add(component);

			return component;
		}

		public T? GetComponent<T>() where T : BaseComponent
		{
			foreach (var component in _components)
			{
				if (component is T)
				{
					return component as T;
				}
			}

			return null;
		}

		public void RemoveComponent<T>() where T : BaseComponent
		{
			_components.RemoveAll(c => c is T);
		}

		public virtual void Start()
		{
			foreach (var component in _components)
			{
				component.Start();
			}
		}

		public virtual void Update(float deltaTime)
		{
			foreach (var component in _components)
			{
				component.Update(deltaTime);
			}
		}

		public abstract void Draw(RenderTarget target, RenderStates states);
	}
}
