using SFML.System;
using Source.Engine.GameObjects;

namespace Source.Engine
{
    public class BaseController : GameObject
    {
        protected GameObject Target { get; private set; }

        public Vector2f Delta { get; protected set; }

        public virtual void SetTarget(GameObject target)
        {
            Target = target;
        }
    }
}