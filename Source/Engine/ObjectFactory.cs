using Source.Engine.GameObjects;
using Source.Engine.Tools;

namespace Source.Engine
{
	public class ObjectFactory
	{
		private static GameLoop GameLoop => _gameLoop ??= Dependency.Get<GameLoop>();
		private static GameLoop _gameLoop;

		private static BaseRenderer Renderer = _renderer ??= Dependency.Get<BaseRenderer>();
		private static BaseRenderer _renderer;

		public static T Instantiate<T>() where T : GameObject, new()
		{
			T instance = new();

			RegisterObject(instance);

			return instance;
		}

		private static void RegisterObject(GameObject gameObject)
		{
			GameLoop.Register(gameObject);
			Renderer.AddRenderElement(gameObject);
		}

		private static void UnregisterObject(GameObject gameObject)
		{
			Renderer.RemoveGameElement(gameObject);
			GameLoop.UnRegister(gameObject);
		}
	}
}
