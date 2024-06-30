using System;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.StatSystem.Defence;
using Source.FightModule.Scripts.StatSystem.Health;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.AbilitySystem.Effects
{
    [Serializable]
    public class AttackEffect : AbilityEffect
    {
        private HealthPresenter _targetHealth;
        private DefencePresenter _targetDefence;

        [field: SerializeField, MinValue(0)] public float Damage { get; private set; }
        [field: SerializeField] public bool IsIgnoredDefence { get; private set; }

        public override void OnStartApply(IUnit caster, IUnit target)
        {
            _targetHealth = target.ComponentContainer.GetComponent<HealthPresenter>();
            _targetDefence = target.ComponentContainer.GetComponent<DefencePresenter>();
        }

        public override void OnApply(IUnit caster, IUnit target)
        {
            float finalDamage = IsIgnoredDefence
                ? Damage
                : _targetDefence.BlockDamage(Damage);

            _targetHealth.RemoveValue(finalDamage);
        }

        public override void OnEndApply(IUnit caster, IUnit target)
        {
        }
    }
}