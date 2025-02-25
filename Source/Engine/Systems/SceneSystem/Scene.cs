namespace Source.Engine.Systems.SceneSystem
{
	public abstract class Scene
	{
		public abstract void Load();

		public abstract void Unload();

		public abstract void Update(float deltaTime);
	}
}