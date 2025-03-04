using Source.Engine.Tools.ProjectUtilities;

namespace Source.Engine.Systems.SceneSystem
{
	public static class SceneLoader
	{
		private const bool LogSceneChaging = true;
		
		private static Dictionary<string, BaseScene> _scenes = new();

		private static BaseScene? _currentActiveScene;

		public static void AddScene<T>() where T : BaseScene, new()
		{
			var sceneName = typeof(T).Name;

			if (!_scenes.TryAdd(sceneName, new T()))
			{
				Debug.LogWarning("Trying to add existing scene!");

				return;
			}
		}

		public static void LoadScene<T>() where T : BaseScene, new()
		{
			var sceneName = typeof(T).Name;

			LoadScene(sceneName);
		}

		public static void LoadScene(string sceneName)
		{
			if (_currentActiveScene != null)
			{
				UnloadCurrentScene();
			}

			if (!_scenes.TryGetValue(sceneName, out var sceneToLoad))
			{
				Debug.LogWarning($"No scene found with name {sceneName}!");
				return;
			}

			_currentActiveScene = sceneToLoad;
			sceneToLoad.LoadInternal();
			LogMessage($"Loaded scene {sceneName}");
		}

		public static void UnloadCurrentScene()
		{
			if (_currentActiveScene == null)
			{
				return;
			}

			var sceneToUnload = _currentActiveScene;
			_currentActiveScene = null;
			sceneToUnload.UnloadInternal();
			LogMessage($"Unloaded scene {sceneToUnload.GetType().Name}");
		}

		public static void Update(float deltaTime) //Deprecated
		{
			_currentActiveScene?.Update(deltaTime);
		}

		private static void LogMessage(object message)
		{
			if (!LogSceneChaging)
			{
				return;
			}
			
			Debug.Log(message);
		}
	}
}