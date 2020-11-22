namespace CoxlinFSM
{
    public abstract class State
    {
        public IStateOwner Owner { private set; get; }
        public State(IStateOwner stateOwner) => Owner = stateOwner;
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnUpdate(float deltaTime);
    }
}
