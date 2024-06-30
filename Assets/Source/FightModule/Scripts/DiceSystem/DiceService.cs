using System;
using System.Collections.Generic;
using System.Linq;
using Source.FightModule.Scripts.Configs;
using Source.FightModule.Scripts.DiceSystem.Container;
using Source.FightModule.Scripts.FaceSystem.Attack;
using Source.FightModule.Scripts.FaceSystem.CriticalHit;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Source.FightModule.Scripts.DiceSystem
{
    [Serializable]
    public class DiceService : IDisposable
    {
        [SerializeField] private RollCountView _rollCountView;
        [SerializeField] private DiceView _diceViewPrefab;
        [SerializeField] private List<DiceConfig> _diceConfigs;
        [SerializeField] private Button _rollButton;
        [SerializeField] private RectTransform _gameArea;
        [SerializeField] private RectTransform _throwStartPoint;
        [SerializeField] private DiceContainerView _diceContainerView;
        [SerializeField] private int _maxRollCount = 3;

        private List<DiceView> _diceViews;
        private List<DicePresenter> _dicePresenters;
        private DiceRoller _diceRoller;
        private DiceContainerPresenter _diceContainerPresenter;

        public void Init()
        {
            _rollButton.onClick.AddListener(OnRollButtonClick);
            _rollCountView.Set(0, _maxRollCount);
            InitDiceContainer();
        }

        public void Dispose()
        {
            _rollButton.onClick.RemoveListener(OnRollButtonClick);
            _diceViews?.ForEach(x => x.Dispose());
            _dicePresenters?.ForEach(x => x.Dispose());
        }

        public void Roll()
        {
            foreach (DicePresenter dicePresenter in _dicePresenters.Where(x => x.IsDiceInHand == false))
            {
                dicePresenter.Roll();
            }
        }

        public void SpawnDices(Transform parent)
        {
            _diceViews = new List<DiceView>(_diceConfigs.Count);
            _dicePresenters = new List<DicePresenter>(_diceConfigs.Count);
            _diceRoller = new DiceRoller(_maxRollCount);

            foreach (DiceConfig diceConfig in _diceConfigs)
            {
                DiceView spawnedDiceView = Object.Instantiate(_diceViewPrefab, parent);
                spawnedDiceView.Init();
                spawnedDiceView.SetDiceIcon(diceConfig.Icon);
                spawnedDiceView.SetName($"{diceConfig.name}-");
                Dice dice = new(diceConfig.Faces, _diceRoller);
                DicePresenter dicePresenter = new(dice, spawnedDiceView, _diceContainerPresenter, _gameArea,
                    _throwStartPoint);

                dice.PutOnTable();
                _diceViews.Add(spawnedDiceView);
                _dicePresenters.Add(dicePresenter);
            }

            if (_dicePresenters.Count(x => x.IsDiceInHand) == 0)
            {
                _diceContainerPresenter.Hide();
            }
            else
            {
                _diceContainerPresenter.Show();
            }
        }

        public void ApplyFaces(IReadOnlyDictionary<Type, IUnit> units)
        {
            float criticalHitModifier = _dicePresenters.FindAll(x => x.CurrentFace is CriticalHit)
                .Sum(x => x.CurrentFace.Config.Value);

            IUnit enemy = units[typeof(Enemy)];
            IUnit player = units[typeof(Player)];

            if (criticalHitModifier > 0)
            {
                foreach (DicePresenter dicePresenter in _dicePresenters)
                {
                    if (dicePresenter.CurrentFace is AttackFace)
                    {
                        dicePresenter.ApplyFace(enemy, criticalHitModifier);
                    }
                    else
                    {
                        dicePresenter.ApplyFace(
                            dicePresenter.CurrentFaceTarget.Value == nameof(Player) ? player : enemy);
                    }
                }
            }
            else
            {
                _dicePresenters.ForEach(x => x.ApplyFace(x.CurrentFaceTarget.Value == nameof(Player)
                    ? units[typeof(Player)]
                    : enemy));
            }
        }

        public void ResetRollsCount()
        {
            _diceRoller.Reset();
            _rollCountView.Set(_diceRoller.CurrentRollCount, _maxRollCount);
        }

        public void ThrowAllDices()
        {
            _dicePresenters.ForEach(x => x.Throw(_throwStartPoint));
        }

        private void OnRollButtonClick()
        {
            if (_diceRoller.CanRollDice == false)
            {
                return;
            }

            _diceRoller.IncreaseRollCount();
            _rollCountView.Set(_diceRoller.CurrentRollCount, _maxRollCount);
            Roll();
        }

        private void InitDiceContainer()
        {
            DiceContainer diceContainer = new(_diceConfigs.Count);
            _diceContainerPresenter = new DiceContainerPresenter(diceContainer, _diceContainerView);
        }
    }
}