using Source.Engine.Tools.ProjectUtilities;

namespace Source.Engine.Systems.SceneSystem
{
	public static class SceneLoader
	{
		private static Dictionary<string, Scene> _scenes = new();

		private static Scene? _currentActiveScene;

		public static void AddScene<T>() where T : Scene, new()
		{
			var sceneName = typeof(T).Name;

			if (!_scenes.TryAdd(sceneName, new T()))
			{
				Debug.LogWarning("Trying to add existing scene!");

				return;
			}
		}

		public static void LoadScene(string sceneName)
		{
			if (!_scenes.TryGetValue(sceneName, out var scene))
			{
				Debug.LogWarning($"No scene in collection with name {sceneName}!");

				return;
			}
			_currentActiveScene = scene;
			scene.LoadInternal();
		}

		public static void LoadScene<T>() where T : Scene, new()
		{
			var sceneName = typeof(T).Name;

			LoadScene(sceneName);
		}

		public static void UnloadScene<T>() where T : Scene, new()
		{
			var sceneName = typeof(T).Name;

			if (!_scenes.TryGetValue(sceneName, out var scene))
			{
				Debug.LogWarning($"No scene in collection with name {sceneName}!");

				return;
			}
			_currentActiveScene = null;
			scene.Unload();
		}

		public static void Update(float deltaTime)
		{
			foreach (var scene in _scenes.Values)
			{
				if (scene.IsActive)
				{
					scene.Update(deltaTime);
				}
			}
		}
	}
}