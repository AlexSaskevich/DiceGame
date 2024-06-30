using Source.FightModule.Scripts.UnitSystem;

namespace Source.FightModule.Scripts.FaceSystem
{
    public interface IFace
    {
        public FaceConfig Config { get; set; }

        public void Apply(IUnit unit, params float[] modifiers);
    }
}