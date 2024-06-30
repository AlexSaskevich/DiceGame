using Source.ComponentContainer;

namespace Source.FightModule.Scripts.UnitSystem
{
    public interface IUnit
    {
        public IComponentContainer ComponentContainer { get; }
    }
}