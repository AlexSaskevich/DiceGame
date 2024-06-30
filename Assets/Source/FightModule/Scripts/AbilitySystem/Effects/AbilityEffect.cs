using System;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.AbilitySystem.Effects
{
    [Serializable]
    public abstract class AbilityEffect
    {
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField, MinValue(1)] public int UseCountPerTurn { get; private set; }
        [field: SerializeField, MinValue(0)] public int Cost { get; private set; }

        public void Apply(IUnit caster, IUnit target)
        {
            OnStartApply(caster, target);

            for (int i = 0; i < UseCountPerTurn; i++)
            {
                OnApply(caster, target);
            }

            OnEndApply(caster, target);
        }

        public abstract void OnStartApply(IUnit caster, IUnit target);
        public abstract void OnApply(IUnit caster, IUnit target);
        public abstract void OnEndApply(IUnit caster, IUnit target);
    }
}