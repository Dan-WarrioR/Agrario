namespace Source.Engine.GameObjects.Components
{
    public abstract class BaseComponent
    {
        protected GameObject Owner { get; private set; }

        public virtual void Start() { }

        public virtual void Update(float deltaTime) { }

        public void SetOwner(GameObject owner)
        {
            Owner = owner;
        }
    }
}