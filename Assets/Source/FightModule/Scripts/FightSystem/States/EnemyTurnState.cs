using System;
using System.Collections.Generic;
using Source.FightModule.Scripts.AbilitySystem;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FightSystem.States
{
    public class EnemyTurnState : BaseState
    {
        private readonly AbilityService _enemyAbilityService;
        private readonly IReadOnlyDictionary<Type, IUnit> _units;

        public EnemyTurnState(FightSystem initializer) : base(initializer)
        {
            _enemyAbilityService = initializer.EnemyAbilityService;
            _units = initializer.Units;
        }

        public override void Enter()
        {
            base.Enter();
            Debug.Log($"Enemy attacked Player!");
            AbilityPresenter abilityPresenter = _enemyAbilityService.GetAbility();

            if (abilityPresenter == null)
            {
                Debug.LogError("Abilities are over!");
                return;
            }

            abilityPresenter.Use(_units[typeof(Enemy)], _units[typeof(Player)]);
        }

        public override void Exit()
        {
            base.Exit();
            
            _enemyAbilityService.ApplyDelayedAbilities(_units[typeof(Enemy)], _units[typeof(Player)]);
            _enemyAbilityService.UpdateTimers();
        }
    }
}