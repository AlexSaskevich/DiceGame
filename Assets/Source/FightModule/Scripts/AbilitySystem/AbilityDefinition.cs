using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using Source.ComponentContainer;
using Source.FightModule.Scripts.StatSystem.Energy;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.AbilitySystem
{
    [CreateAssetMenu(menuName = Constants.AbilitiesConfigs + nameof(Ability), fileName = nameof(Ability))]
    public class AbilityDefinition : Ability
    {
        [ReadOnly, ShowInInspector] public int TotalCost => Effects.Sum(x => x.Cost);

        public override HashSet<AbilityStatus> CurrentStatuses { get; protected set; } = new();

        public override void Use(IUnit caster, IUnit target)
        {
            if (Validate(caster) == false)
            {
                return;
            }

            Effects.ForEach(x => x.Apply(caster, target));

            AbilityStatus abilityStatus = Cooldown <= 0 ? AbilityStatus.Ready : AbilityStatus.Cooldown;

            SetStatus(abilityStatus);
        }

        public override void UseDelayed(IUnit caster, IUnit target)
        {
            if (Validate(caster) == false)
            {
                return;
            }

            AbilityStatus abilityStatus = Cooldown <= 0 ? AbilityStatus.Ready : AbilityStatus.Cooldown;

            if (CurrentStatuses.Contains(AbilityStatus.Applying))
            {
                SetStatus(abilityStatus, AbilityStatus.Ready, AbilityStatus.Applying);
                return;
            }

            SetStatus(abilityStatus, AbilityStatus.Applying);
        }

        private bool Validate(IUnit caster)
        {
            if (caster is Enemy)//todo debug only!
            {
                return true;
            }

            IComponentContainer casterComponentContainer = caster.ComponentContainer;
            EnergyPresenter casterEnergy = casterComponentContainer.GetComponent<EnergyPresenter>();

            if (CanUse(casterEnergy) == false)
            {
                return false;
            }

            casterEnergy.RemoveValue(TotalCost);
            return true;
        }

        private bool CanUse(EnergyPresenter energyPresenter)
        {
            if (CurrentStatuses.Contains(AbilityStatus.Cooldown))
            {
                Debug.LogError($"Cooldown! {name}");
                return false;
            }

            if (energyPresenter.CurrentStatValue < TotalCost)
            {
                Debug.LogError($"Not enough energy! {GetType().Name}");
                SetStatus(AbilityStatus.NeedEnergy);
                return false;
            }

            return true;
        }
    }
}