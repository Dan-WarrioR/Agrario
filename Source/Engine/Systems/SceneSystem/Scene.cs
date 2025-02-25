namespace Source.Engine.Systems.SceneSystem
{
	public abstract class Scene
	{
		public bool IsActive { get; private set; } = false;

		internal virtual void LoadInternal()
		{
			IsActive = true;
			Load();
		}

		internal virtual void UnloadInternal()
		{
			IsActive = false;
			Unload();
		}

		public abstract void Load();
		public abstract void Unload();
		public virtual void Update(float deltaTime) { }
	}
}