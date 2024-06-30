using System;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.StatSystem;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.AbilitySystem.Effects
{
    [Serializable]
    public class DeBuffStat : AbilityEffect
    {
        [SerializeField] private StatPresenter _stat;
        [SerializeField, MinValue(0)] private float _modifier;

        private StatPresenter _targetStat;

        public override void OnStartApply(IUnit caster, IUnit target)
        {
            _targetStat = target.ComponentContainer.GetComponent<StatPresenter>(_stat.GetType());
        }

        public override void OnApply(IUnit caster, IUnit target)
        {
            _targetStat.RemoveValue(_modifier);
        }

        public override void OnEndApply(IUnit caster, IUnit target)
        {
        }
    }
}