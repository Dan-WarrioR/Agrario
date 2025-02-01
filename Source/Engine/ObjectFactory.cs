using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Engine
{
	public class ObjectFactory
	{
		private GameLoop GameLoop => _gameLoop ??= Dependency.Get<GameLoop>();
		private GameLoop _gameLoop;

		private BaseRenderer _renderer;

		public ObjectFactory(BaseRenderer renderer)
		{
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
			GameLoop.Register(gameObject);
			_renderer.AddRenderElement(gameObject);
		}

		private void UnregisterObject(GameObject gameObject)
		{
			_renderer.RemoveGameElement(gameObject);
			GameLoop.UnRegister(gameObject);
		}
	}
}
