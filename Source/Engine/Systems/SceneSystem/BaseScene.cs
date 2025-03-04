using Source.Engine.GameObjects;

namespace Source.Engine.Systems.SceneSystem
{
	public abstract class BaseScene
	{
		private HashSet<GameObject> _sceneObejcts = new();

		internal virtual void LoadInternal()
		{
			ObjectFactory.OnObjectCreated += RegisterSceneObject;
			Load();
		}

		internal virtual void UnloadInternal()
		{
			ObjectFactory.OnObjectCreated -= RegisterSceneObject;
			foreach (var obj in _sceneObejcts)
			{
				obj.Destroy();
			}
			_sceneObejcts.Clear();

			Unload();
		}

		public abstract void Load();
		public abstract void Unload();
		public virtual void Update(float deltaTime) { }

		private void RegisterSceneObject(GameObject gameObject)
		{
			if (!_sceneObejcts.Contains(gameObject))
			{
				_sceneObejcts.Add(gameObject);
			}
		}
	}
}