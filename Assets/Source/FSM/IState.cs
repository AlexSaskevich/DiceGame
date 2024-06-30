namespace Source.FSM
{
    public interface IState<out T>
    {
        public T Initializer { get; }
        public void Enter();
        public void Update();
        public void Exit();
    }
}