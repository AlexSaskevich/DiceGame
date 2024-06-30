using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.UnitSystem;
using Source.FightModule.Scripts.UnitSystem.Configs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.FightModule.Scripts.AbilitySystem
{
    [Serializable]
    public class AbilityService : IDisposable
    {
        [SerializeField] private List<AbilityDefinition> _abilitiesForTest;
        [SerializeField] private AbilityView _abilityViewPrefab;

        private List<AbilityPresenter> _abilities = new();
        private IEnumerator<AbilityPresenter> _enumerator;

        public void Init(Dictionary<Type, IUnit> units, UnitSpawnData unitSpawnData)
        {
            foreach (AbilityDefinition abilityDefinition in _abilitiesForTest)
            {
                AbilityView view = Object.Instantiate(_abilityViewPrefab, unitSpawnData.AbilityViewsContainer);
                view.Init();
                view.Set(abilityDefinition.Icon, abilityDefinition.name, abilityDefinition.TotalCost.ToString(),
                    abilityDefinition.Cooldown, abilityDefinition.Duration);
                AbilityPresenter abilityPresenter = new(abilityDefinition, view, units);
                _abilities.Add(abilityPresenter);
            }

            _enumerator = _abilities.GetEnumerator();
        }

        public void Dispose()
        {
            _abilities?.ForEach(x => x?.Dispose());
        }

        public void UpdateTimers()
        {
            foreach (AbilityPresenter ability in _abilities.Where(x =>
                         x.CurrentStatuses.Contains(AbilityStatus.Cooldown) ||
                         x.CurrentStatuses.Contains(AbilityStatus.Applying)))
            {
                ability.UpdateTimers();
            }
        }

        public void ApplyDelayedAbilities(IUnit caster, IUnit target)
        {
            foreach (AbilityPresenter ability in _abilities.Where(x =>
                         x.IsDelayed && x.CurrentStatuses.Contains(AbilityStatus.Applying)))
            {
                Debug.LogError($"Apply {ability.Name}");
                ability.ApplyEffects(caster, target);
            }
        }

        public AbilityPresenter GetAbility()
        {
            return _enumerator.MoveNext() == false ? null : _enumerator.Current;
        }

        [Button]
        public void CancelAbilities()
        {
            foreach (AbilityPresenter abilityPresenter in _abilities.Where(x => x.CanCanceled))
            {
                abilityPresenter.Cancel();
            }
        }
    }
}