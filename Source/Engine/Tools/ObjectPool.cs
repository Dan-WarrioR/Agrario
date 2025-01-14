using Source.Engine.GameObjects;

namespace Source.Engine.Tools
{
	public class ObjectPool<T> where T : GameObject
	{
		public IReadOnlyList<T> Items => _pool;

		private readonly List<T> _pool;

		private readonly Func<T> _spawnFunc;
		private readonly Action<T> _respawnAction;

		public ObjectPool(int initialSize, int maxSize, Func<T> spawnFunc, Action<T> respawnAction)
		{
			_pool = new(maxSize);

			_spawnFunc = spawnFunc;
			_respawnAction = respawnAction;

			for (int i = 0; i < initialSize; i++)
			{
				_pool.Add(SpawnObject());
			}
		}

		public T SpawnObject()
		{
			var item = _spawnFunc();

			_pool.Add(item);

			return item;
		}

		public void RespawnFirstObject()
		{
			foreach (var item in _pool)
			{
				if (!item.IsActive)
				{
					_respawnAction(item);

					return;
				}
			}
		}

		public void RespawanAll()
		{
			foreach (var item in _pool)
			{
				_respawnAction(item);
			}
		}

		public bool TryGetElement(out T element)
		{
			foreach (var item in _pool)
			{
				if (item.IsActive)
				{
					element = item;
					return true;
				}
			}

			element = null;
			return false;
		}

		public bool HasFreeItem()
		{
			foreach (var item in _pool)
			{
				if (item.IsActive)
				{
					return true;
				}
			}

			return false;
		}

		public void DestroyObject(T gameObject)
		{
			gameObject.SetActive(false);
		}
	}
}