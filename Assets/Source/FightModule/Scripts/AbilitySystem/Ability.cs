using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.AbilitySystem.Effects;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.AbilitySystem
{
    public abstract class Ability : SerializedScriptableObject
    {
        [SerializeField] private readonly List<AbilityEffect> _effects = new();

        [field: SerializeField, MinValue(0)] public int Cooldown { get; private set; }
        [field: SerializeField, MinValue(0)] public int Duration { get; private set; }

        [field: SerializeField, ShowIf("@Duration > 0")]
        public bool CanCanceled { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }

        protected IReadOnlyList<AbilityEffect> Effects => _effects;

        public abstract HashSet<AbilityStatus> CurrentStatuses { get; protected set; }
        public bool IsDelayed => Duration > 0;

        public abstract void Use(IUnit caster, IUnit target);
        public abstract void UseDelayed(IUnit caster, IUnit target);

        public void SetStatus(params AbilityStatus[] statuses)
        {
            if (statuses.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(statuses));
            }

            CurrentStatuses.RemoveWhere(x => statuses.Contains(x) == false);

            foreach (AbilityStatus abilityStatus in statuses)
            {
                CurrentStatuses.Add(abilityStatus);
            }
        }

        public void RemoveStatus(params AbilityStatus[] statuses)
        {
            if (statuses.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(statuses));
            }

            foreach (AbilityStatus abilityStatus in statuses)
            {
                CurrentStatuses.Remove(abilityStatus);
            }
        }

        public void AddStatus(params AbilityStatus[] statuses)
        {
            if (statuses.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(statuses));
            }

            foreach (AbilityStatus abilityStatus in statuses)
            {
                CurrentStatuses.Add(abilityStatus);
            }
        }

        public void ApplyEffects(IUnit caster, IUnit target)
        {
            _effects.ForEach(x => x.Apply(caster, target));
        }
    }
}