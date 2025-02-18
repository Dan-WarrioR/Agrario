namespace Source.Engine.Systems.Animation
{
    public abstract class AnimationData
    {
        protected List<AnimationState> States { get; } = new();
        protected List<AnimationTransition> Transitions { get; } = new();
        
        public AnimationState InitialState { get; protected set; }

        public List<AnimationState> GetStates()
        {
            return States;
        }

        public List<AnimationTransition> GetTransitions()
        {
            return Transitions;
        }
    }
}