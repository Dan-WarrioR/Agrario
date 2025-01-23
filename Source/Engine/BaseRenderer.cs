using Source.Engine.GameObjects;

namespace Source.Engine
{
	public interface IUIElement
	{

	}

	public abstract class BaseRenderer
	{
		protected List<GameObject> GameElements = new();

		protected List<GameObject> UIElements = new();

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
