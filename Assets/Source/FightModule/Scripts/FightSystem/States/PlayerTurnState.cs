using System;
using System.Collections.Generic;
using Source.FightModule.Scripts.DiceSystem;
using Source.FightModule.Scripts.UnitSystem;

namespace Source.FightModule.Scripts.FightSystem.States
{
    public class PlayerTurnState : BaseState
    {
        private readonly DiceService _diceService;
        private readonly FightSystem _initializer;
        private readonly IReadOnlyDictionary<Type, IUnit> _units;

        public PlayerTurnState(FightSystem initializer, DiceService diceService) : base(initializer)
        {
            _initializer = initializer;
            _units = _initializer.Units;
            _diceService = diceService;
            _diceService.SpawnDices(_initializer.HorizontalLayoutGroup);
            _diceService.Roll();
        }

        public override void Enter()
        {
            base.Enter();
            _diceService.ThrowAllDices();
            _diceService.Roll();
        }

        public override void Exit()
        {
            base.Exit();
            _diceService.ApplyFaces(_units);
            _initializer.IsEndTurnButtonClicked = false;
            _diceService.ResetRollsCount();
            _initializer.PlayerAbilityService.ApplyDelayedAbilities(_units[typeof(Player)], _units[typeof(Enemy)]);
            _initializer.PlayerAbilityService.UpdateTimers();
        }
    }
}