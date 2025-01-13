using Source.Engine.GameObjects;

namespace Source.Engine
{
	public abstract class BaseRenderer
	{
		protected List<GameObject> Drawables = new();

		public void Add(GameObject gameobject)
		{
			if (Drawables.Contains(gameobject))
			{
				return;
			}

			Drawables.Add(gameobject);
		}

		public void Remove(GameObject gameobject)
		{
			Drawables.Remove(gameobject);
		}

		public abstract void Render();
	}
}
