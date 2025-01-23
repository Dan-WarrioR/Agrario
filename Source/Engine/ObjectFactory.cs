using Source.Engine.GameObjects;

namespace Source.Engine
{
	public class ObjectFactory
	{
		private GameLoop _gameLoop;
		private BaseRenderer _renderer;

		public ObjectFactory(GameLoop gameLoop, BaseRenderer renderer)
		{
			_gameLoop = gameLoop;
			_renderer = renderer;
		}

		public T Instantiate<T>() where T : GameObject, new()
		{
			T instance = new();

			RegisterObject(instance);

			return instance;
		}

		private void RegisterObject(GameObject gameObject)
		{
			_gameLoop.Register(gameObject);
			_renderer.AddRenderElement(gameObject);
		}

		private void UnregisterObject(GameObject gameObject)
		{
			_renderer.RemoveGameElement(gameObject);
			_gameLoop.UnRegister(gameObject);
		}
	}
}
