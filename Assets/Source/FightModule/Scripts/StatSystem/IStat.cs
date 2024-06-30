using System;

namespace Source.FightModule.Scripts.StatSystem
{
    public interface IStat
    {
        public event Action<float> Changed;

        public float CurrentValue { get; }
        public float DefaultValue { get; }

        public void AddValue(float value);
        public void RemoveValue(float value);
    }
}