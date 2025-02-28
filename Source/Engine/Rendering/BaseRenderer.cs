using SFML.Graphics;
using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Engine.Rendering
{
	public interface IUIElement
	{

	}

	public abstract class BaseRenderer
	{
		protected List<GameObject> GameElements = new();

		protected List<GameObject> UIElements = new();

		protected RenderWindow Window { get; private set; }

		public virtual void Initialize(RenderWindow window)
		{
			Dependency.Register(this);

			Window = window;
		}

		~BaseRenderer()
		{
			Dependency.Unregister(this);
		}

		public void AddRenderElement(GameObject gameObject)
		{
			if (gameObject is IUIElement)
			{
				if (!UIElements.Contains(gameObject))
				{
					UIElements.Add(gameObject);
				}

				return;
			}

			if (!GameElements.Contains(gameObject))
			{
				GameElements.Add(gameObject);
			}
		}

		public void RemoveGameElement(GameObject gameObject)
		{
			if (gameObject is IUIElement)
			{
				UIElements.Remove(gameObject);
				return;
			}

			GameElements.Remove(gameObject);
		}

		public abstract void Render();
	}
}
