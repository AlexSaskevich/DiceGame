using Source.ComponentContainer;

namespace Source.FightModule.Scripts.UnitSystem
{
    public abstract class Unit : IUnit
    {
        protected Unit(IComponentContainer componentContainer)
        {
            ComponentContainer = componentContainer;
        }

        public IComponentContainer ComponentContainer { get; private set; }
    }
}