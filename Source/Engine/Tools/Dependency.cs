namespace Source.Engine.Tools
{
	public static class Dependency
	{
		private static Dictionary<Type, object> _instances = new();

		public static T Get<T>() where T : class
		{
			var type = typeof(T);
			foreach (var pair in _instances)
			{
				if (pair.Key == type)
				{
					return pair.Value as T;
				}
			}

			return null;
		}

		public static void Register<T>(T instance) where T : class
		{
			Register(typeof(T), instance);
		}

		public static void Register(Type type, object instance)
		{
			if (instance == null)
			{
				throw new NullReferenceException($"Dependency Instance cannot be null.");
			}

			if (!_instances.TryAdd(type, instance))
			{
				throw new ArgumentException($"{type.Name} is already registered. Only one instance per type is allowed.");
			}
		}

		public static bool TryRegister<T>(T instance) where T : class
		{
			return TryRegister(typeof(T), instance);
		}

		public static bool TryRegister(Type type, object instance)
		{
			if (instance == null)
			{
				throw new NullReferenceException($"Dependency Instance cannot be null.");
			}

			return _instances.TryAdd(type, instance);
		}

		public static void Unregister<T>(T instance) where T : class
		{
			Unregister(typeof(T), instance);
		}

		public static void Unregister(Type type, object instance)
		{
			if (instance == null)
			{
				throw new NullReferenceException($"Dependency Instance cannot be null.");
			}

			if (!_instances.ContainsKey(type))
			{
				throw new ArgumentException($"{type.Name} was not registered. Instance must be registered before unregister.");
			}

			_instances.Remove(type);
		}

		public static bool TryUnregister<T>(T instance) where T : class
		{
			return TryUnregister(typeof(T), instance);
		}

		public static bool TryUnregister(Type type, object instance)
		{
			if (instance == null)
			{
				throw new NullReferenceException($"Dependency Instance cannot be null.");
			}

			if (!_instances.ContainsKey(type))
			{
				return false;
			}

			_instances.Remove(type);
			return true;
		}
	}
}