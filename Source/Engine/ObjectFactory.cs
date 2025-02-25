using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Engine
{
	public class ObjectFactory
	{
		private GameLoop GameLoop => _gameLoop ??= Dependency.Get<GameLoop>();
		private GameLoop _gameLoop;

		private static BaseRenderer Renderer = _renderer ??= Dependency.Get<BaseRenderer>();
		private static BaseRenderer _renderer;

		public T Instantiate<T>() where T : GameObject, new()
		{
			T instance = new();

			RegisterObject(instance);

			return instance;
		}

		private void RegisterObject(GameObject gameObject)
		{
			GameLoop.Register(gameObject);
			Renderer.AddRenderElement(gameObject);
		}

		private void UnregisterObject(GameObject gameObject)
		{
			Renderer.RemoveGameElement(gameObject);
			GameLoop.UnRegister(gameObject);
		}
	}
}
