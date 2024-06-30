using System;
using System.Collections.Generic;
using Source.ComponentContainer;
using Source.FightModule.Scripts.AbilitySystem;
using Source.FightModule.Scripts.StatSystem;
using Source.FightModule.Scripts.StatSystem.Defence;
using Source.FightModule.Scripts.StatSystem.Energy;
using Source.FightModule.Scripts.StatSystem.Health;
using Source.FightModule.Scripts.UnitSystem;
using Source.FightModule.Scripts.UnitSystem.Configs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Source.FightModule.Scripts.FightSystem.States
{
    public class InitFightState : BaseState
    {
        private readonly Dictionary<Type, IUnit> _units;
        private readonly AbilityService _playerAbilityService;
        private readonly AbilityService _enemyAbilityService;

        public InitFightState(FightSystem initializer, in Dictionary<Type, IUnit> units) : base(initializer)
        {
            _units = units;

            _playerAbilityService =
                InitAbilityService(initializer.PlayerAbilityService, units, initializer.PlayerSpawnData);

            _enemyAbilityService =
                InitAbilityService(initializer.EnemyAbilityService, units, initializer.EnemySpawnData);


            InitUnit(initializer.PlayerSpawnData, typeof(Player));
            InitUnit(initializer.EnemySpawnData, typeof(Enemy));
        }

        private void InitUnit(UnitSpawnData unitSpawnData, Type unitType)
        {
            IComponentContainer componentContainer = unitType == typeof(Player)
                ? CreatePlayerComponentContainer(unitSpawnData)
                : CreateEnemyComponentContainer(unitSpawnData);

            IUnit unit = CreateUnitInstance(unitType, componentContainer);

            SpawnUnitView(unitSpawnData.UnitView, unitSpawnData.Parent);
            _units[unit.GetType()] = unit;
        }

        private AbilityService InitAbilityService(AbilityService abilityService, Dictionary<Type, IUnit> units,
            UnitSpawnData unitSpawnData)
        {
            abilityService.Init(units, unitSpawnData);
            return abilityService;
        }

        private TPresenter CreatePresenter<TPresenter, TModel>(UnitSpawnData unitSpawnData,
            Func<UnitConfig, TModel> createModel,
            Func<TModel, StatView, TPresenter> createPresenter)
            where TPresenter : StatPresenter
            where TModel : class
        {
            StatView statView = Object.Instantiate(unitSpawnData.StatViewsConfig.GetViewForType<TModel>(),
                unitSpawnData.Parent);
            TModel model = createModel(unitSpawnData.Config);
            return createPresenter(model, statView);
        }

        private IComponentContainer CreatePlayerComponentContainer(UnitSpawnData unitSpawnData)
        {
            HealthPresenter healthPresenter = CreatePresenter(unitSpawnData, CreateHealth,
                (health, statView) => new HealthPresenter(health, statView));
            DefencePresenter defencePresenter = CreatePresenter(unitSpawnData, CreateDefence,
                (defence, statView) => new DefencePresenter(defence, statView));
            EnergyPresenter energyPresenter = CreatePresenter(unitSpawnData, CreateEnergy,
                (energy, statView) => new EnergyPresenter(energy, statView));

            IComponentContainer componentContainer = new ComponentContainer.ComponentContainer();
            componentContainer
                .AddComponent(healthPresenter)
                .AddComponent(defencePresenter)
                .AddComponent(energyPresenter);

            return componentContainer;
        }

        private IComponentContainer CreateEnemyComponentContainer(UnitSpawnData unitSpawnData)
        {
            HealthPresenter healthPresenter =
                CreatePresenter(unitSpawnData, CreateHealth, (health, view) => new HealthPresenter(health, view));
            DefencePresenter defencePresenter = CreatePresenter(unitSpawnData, CreateDefence,
                (defence, view) => new DefencePresenter(defence, view));

            IComponentContainer componentContainer = new ComponentContainer.ComponentContainer();
            componentContainer
                .AddComponent(healthPresenter)
                .AddComponent(defencePresenter)
                .AddComponent(_enemyAbilityService);

            return componentContainer;
        }

        private Defence CreateDefence(UnitConfig config)
        {
            return new Defence(config.DefaultDefence, config.DefaultDefence);
        }

        private Health CreateHealth(UnitConfig config)
        {
            return new Health(config.DefaultHealth, config.DefaultHealth);
        }

        private Energy CreateEnergy(UnitConfig config)
        {
            PlayerConfig playerConfig = (PlayerConfig)config;
            return new Energy(playerConfig.DefaultEnergy, playerConfig.DefaultEnergy);
        }

        private IUnit CreateUnitInstance(Type unitType, IComponentContainer componentContainer)
        {
            if (unitType == typeof(Enemy))
            {
                return new Enemy(componentContainer);
            }

            if (unitType == typeof(Player))
            {
                return new Player(componentContainer);
            }

            throw new ArgumentException($"Unknown unit type: {unitType.Name}");
        }

        private void SpawnUnitView(UnitView unitView, Transform parent)
        {
            UnitView spawned = Object.Instantiate(unitView, parent);
            spawned.transform.SetAsFirstSibling();
        }
    }
}