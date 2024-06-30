using Source.FSM;
using UnityEngine;

namespace Source.FightModule.Scripts.FightSystem.States
{
    public abstract class BaseState : IState<FightSystem>
    {
        protected BaseState(FightSystem initializer)
        {
            Initializer = initializer;
        }

        public FightSystem Initializer { get; }

        public virtual void Enter()
        {
            Debug.LogWarning($"Enter in {GetType().Name}");
        }

        public virtual void Update()
        {
        }

        public virtual void Exit()
        {
            Debug.LogError($"Exit from {GetType().Name}");
        }
    }
}