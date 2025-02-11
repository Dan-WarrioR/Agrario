namespace Source.Engine.Tools
{
	public static class Dependency
	{
		private static Dictionary<Type, object> _instances = new();

		public static T Get<T>() where T : class
		{
			var type = typeof(T);

			if (_instances.TryGetValue(type, out var instance))
			{
				return instance as T;
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
	}
}