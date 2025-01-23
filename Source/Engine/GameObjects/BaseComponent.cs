namespace Source.Engine.GameObjects
{
	public abstract class BaseComponent
	{
		protected GameObject Owner { get; private set; }

		public virtual void Update(float deltaTime) { }

		public void SetOwner(GameObject owner)
		{
			Owner = owner;
		}
	}
}