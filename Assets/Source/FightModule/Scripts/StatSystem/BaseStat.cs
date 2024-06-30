using System;
using UnityEngine;

namespace Source.FightModule.Scripts.StatSystem
{
    public abstract class BaseStat : IStat
    {
        protected BaseStat(float currentValue, float defaultValue)
        {
            CurrentValue = currentValue;
            DefaultValue = defaultValue;
        }

        public event Action<float> Changed;

        public float CurrentValue { get; protected set; }
        public float DefaultValue { get; protected set; }

        public virtual void AddValue(float value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            CurrentValue = Mathf.Clamp(CurrentValue + value, 0, DefaultValue);
            Changed?.Invoke(CurrentValue);

            OnAddValue();
        }

        public void RemoveValue(float value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            CurrentValue = Mathf.Clamp(CurrentValue - value, 0, DefaultValue);
            Changed?.Invoke(CurrentValue);

            OnRemoveValue();
        }

        public abstract void OnAddValue();
        public abstract void OnRemoveValue();
    }
}