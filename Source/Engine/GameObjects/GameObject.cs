using SFML.Graphics;
using SFML.System;
using Source.Engine.GameObjects.Components;

namespace Source.Engine.GameObjects
{
    public interface IActivable
	{
		public bool IsActive { get; }

		public void SetActive(bool isActive);
	}

	public abstract class GameObject : Drawable, IActivable
	{
		public bool IsActive { get; private set; } = true;

		public Vector2f InitialPosition { get; private set; }

		private Dictionary<Type, BaseComponent> _components = new();

		public void Initialize(Vector2f initialPosition)
		{
			InitialPosition = initialPosition;
		}

		public void SetActive(bool isActive)
		{
			IsActive = isActive;
		}

		public void Start()
		{
			foreach (var component in _components.Values)
			{
				component.Start();
			}

			OnStart();
		}

		public void Update(float deltaTime)
		{
			foreach (var component in _components.Values)
			{
				component.Update(deltaTime);
			}

			OnUpdate(deltaTime);
		}

		public virtual void OnStart() { }
		public virtual void OnUpdate(float deltaTime) { }

		public virtual void Draw(RenderTarget target, RenderStates states)
		{

		}

		#region Components Interfaces

		public T AddComponent<T>() where T : BaseComponent, new()
		{
			T component = new();

			component.SetOwner(this);
			_components.Add(component.GetType(), component);

			return component;
		}

		public T AddComponent<T>(T component) where T : BaseComponent
		{
			component.SetOwner(this);
			_components.Add(component.GetType(), component);

			return component;
		}

		public T? GetComponent<T>() where T : BaseComponent
		{
			_components.TryGetValue(typeof(T), out BaseComponent component);

			return component as T;
		}

		public bool TryGetComponent<T>(out T component) where T : BaseComponent
		{
			component = GetComponent<T>();

			return component != null;
		}

		public void RemoveComponent<T>() where T : BaseComponent
		{
			_components.Remove(typeof(T));
		}

		#endregion
	}
}
