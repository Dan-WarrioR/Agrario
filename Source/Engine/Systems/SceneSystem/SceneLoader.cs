using Source.Engine.Tools.ProjectUtilities;

namespace Source.Engine.Systems.SceneSystem
{
	public static class SceneLoader
	{
		private static Dictionary<string, Scene> _scenes = new();

		private static Scene? _currentScene;

		public static void AddScene<T>() where T : Scene, new()
		{
			var sceneName = typeof(T).Name;

			if (!_scenes.TryAdd(sceneName, new T()))
			{
				Debug.LogWarning("Trying to add existing scene!");

				return;
			}
		}

		public static void LoadScene<T>() where T : Scene, new()
		{
			var sceneName = typeof(T).Name;
			Debug.Log($"Loaded {sceneName}");
			if (!_scenes.TryGetValue(sceneName, out var scene))
			{
				Debug.LogWarning($"No scene in collection with name {sceneName}!");

				return;
			}
			_currentScene = scene;
			scene.Load();
		}

		public static void UnloadScene<T>() where T : Scene, new()
		{
			var sceneName = typeof(T).Name;
			Debug.Log($"Exit {sceneName}");
			if (!_scenes.TryGetValue(sceneName, out var scene))
			{
				Debug.LogWarning($"No scene in collection with name {sceneName}!");

				return;
			}
			_currentScene = null;
			scene.Unload();

			_scenes.Remove(sceneName);
		}

		public static void Update(float deltaTime)
		{
			_currentScene?.Update(deltaTime);
		}
	}
}