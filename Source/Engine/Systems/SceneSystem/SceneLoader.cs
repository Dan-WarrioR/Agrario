﻿using Source.Engine.Tools.ProjectUtilities;

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

		public static void LoadScene(string sceneName)
		{
			if (!_scenes.TryGetValue(sceneName, out var scene))
			{
				Debug.LogWarning($"No scene in collection with name {sceneName}!");

				return;
			}
			_currentActiveScene = scene;
			scene.LoadInternal();
			LogMessage($"Loading scene {sceneName}");
		}

		public static void LoadScene<T>() where T : BaseScene, new()
		{
			var sceneName = typeof(T).Name;

			LoadScene(sceneName);
		}

		public static void UnloadScene<T>() where T : BaseScene, new()
		{
			var sceneName = typeof(T).Name;

			if (!_scenes.TryGetValue(sceneName, out var scene))
			{
				Debug.LogWarning($"No scene in collection with name {sceneName}!");

				return;
			}
			_currentActiveScene = null;
			scene.Unload();
			LogMessage($"Unloading scene {sceneName}");
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