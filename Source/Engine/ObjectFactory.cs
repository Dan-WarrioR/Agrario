﻿using SFML.System;
using Source.Engine.GameObjects;
using Source.Engine.Rendering;
using Source.Engine.Tools;

namespace Source.Engine
{
	public class ObjectFactory
	{
		private static GameLoop GameLoop => _gameLoop ??= Dependency.Get<GameLoop>();
		private static GameLoop _gameLoop;

		private static BaseRenderer Renderer = _renderer ??= Dependency.Get<BaseRenderer>();
		private static BaseRenderer _renderer;

		internal static event Action<GameObject> OnObjectCreated;

		public static T Instantiate<T>() where T : GameObject, new()
		{
			return Instantiate<T>(new(0, 0));
		}

		public static T Instantiate<T>(Vector2f position) where T : GameObject, new()
		{
			T instance = new();

			instance.Initialize(position);

			RegisterObject(instance);

			OnObjectCreated?.Invoke(instance);

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
