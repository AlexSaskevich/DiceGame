using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.AbilitySystem;
using Source.FightModule.Scripts.DiceSystem;
using Source.FightModule.Scripts.FightSystem.States;
using Source.FightModule.Scripts.StatSystem.Defence;
using Source.FightModule.Scripts.StatSystem.Energy;
using Source.FightModule.Scripts.StatSystem.Health;
using Source.FightModule.Scripts.UnitSystem;
using Source.FightModule.Scripts.UnitSystem.Configs;
using Source.FSM;
using UnityEngine;
using UnityEngine.UI;

namespace Source.FightModule.Scripts.FightSystem
{
    public class FightSystem : SerializedMonoBehaviour
    {
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private DiceService _diceService;

        private StateMachine<FightSystem> _stateMachine;

        [field: SerializeField] public AbilityService PlayerAbilityService { get; private set; }
        [field: SerializeField] public AbilityService EnemyAbilityService { get; private set; }
        [field: SerializeField] public UnitSpawnData PlayerSpawnData { get; private set; }
        [field: SerializeField] public UnitSpawnData EnemySpawnData { get; private set; }
        [field: SerializeField] public RectTransform HorizontalLayoutGroup { get; private set; }

        public IReadOnlyDictionary<Type, IUnit> Units { get; } = new Dictionary<Type, IUnit>();
        public bool IsEndTurnButtonClicked { get; set; }

        private void Start()
        {
            _endTurnButton.onClick.AddListener(OnEndTurnButtonClicked);

            CreateStates();
            AddTransitions();

            _stateMachine.SetState<InitFightState>();
        }

        private void OnDestroy()
        {
            _endTurnButton.onClick.RemoveListener(OnEndTurnButtonClicked);
            _diceService?.Dispose();

            foreach (KeyValuePair<Type, IUnit> unit in Units)
            {
                unit.Value.ComponentContainer.GetComponent<HealthPresenter>().Dispose();
            }
        }

        private void AddTransitions()
        {
            _stateMachine.AddTransition<InitFightState, PlayerTurnState>(() => true);
            _stateMachine.AddTransition<PlayerTurnState, EnemyTurnState>(() => IsEndTurnButtonClicked);
            _stateMachine.AddTransition<EnemyTurnState, PlayerTurnState>(() => true);
            _stateMachine.AddAnyTransition<LoseState>(() => IsAlive(Units[typeof(Player)]) == false);
            _stateMachine.AddAnyTransition<WinState>(() => IsAlive(Units[typeof(Enemy)]) == false);
        }

        private void OnEndTurnButtonClicked()
        {
            IsEndTurnButtonClicked = true;
        }

        private bool IsAlive(IUnit unit)
        {
            return unit.ComponentContainer.GetComponent<HealthPresenter>().IsAlive;
        }

        private void CreateStates()
        {
            _diceService.Init();

            InitFightState initFightState = new(this, (Dictionary<Type, IUnit>)Units);
            PlayerTurnState playerTurnState = new(this, _diceService);
            EnemyTurnState enemyTurnState = new(this);
            WinState winState = new(this);
            LoseState loseState = new(this);

            _stateMachine =
                new StateMachine<FightSystem>(initFightState, playerTurnState, enemyTurnState, winState, loseState);
        }

        private void Update()
        {
            _stateMachine.Run();
        }

        #region DebugMethods

        [Button]
        private void ShowCurrentState()
        {
            Debug.Log(_stateMachine.CurrentState.GetType().Name);
        }

        [Button]
        private void DealDamageToPlayer(float damage)
        {
            Units[typeof(Player)].ComponentContainer.GetComponent<HealthPresenter>().RemoveValue(damage);
        }

        [Button]
        private void DealDamageToEnemy(float damage)
        {
            Units[typeof(Enemy)].ComponentContainer.GetComponent<HealthPresenter>().RemoveValue(damage);
        }

        [Button]
        private void RemoveValue(float value)
        {
            Units[typeof(Player)].ComponentContainer.GetComponent<EnergyPresenter>().RemoveValue(value);
            Units[typeof(Player)].ComponentContainer.GetComponent<DefencePresenter>().RemoveValue(value);
            Units[typeof(Player)].ComponentContainer.GetComponent<HealthPresenter>().RemoveValue(value);
        }

        #endregion
    }
}