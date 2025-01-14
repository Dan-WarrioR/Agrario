using Source.Engine.GameObjects;

namespace Source.Engine
{
	public abstract class BaseRenderer
	{
		protected List<GameObject> GameElements = new();

		protected List<GameObject> UIElements = new();

		public void AddGameElement(GameObject gameobject)
		{
			if (GameElements.Contains(gameobject))
			{
				return;
			}

			GameElements.Add(gameobject);
		}

		public void RemoveGameElement(GameObject gameobject)
		{
			GameElements.Remove(gameobject);
		}

		public void AddUIElement(GameObject uiElement)
		{
			if (UIElements.Contains(uiElement))
			{
				return;
			}

			UIElements.Add(uiElement);
		}

		public void RemoveUIElement(GameObject uiElement)
		{
			UIElements.Remove(uiElement);
		}

		public abstract void Render();
	}
}
