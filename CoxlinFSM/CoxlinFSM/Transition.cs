namespace CoxlinFSM
{
    public abstract class Transition
    {
        public State StateToTransitionTo;
        public abstract bool ShouldTransitionToState();
    }
}
