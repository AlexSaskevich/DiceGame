using System;
using Source.FightModule.Scripts.DiceSystem.Container;
using Source.FightModule.Scripts.FaceSystem;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem
{
    public class DicePresenter : IDisposable
    {
        private readonly Dice _dice;
        private readonly DiceView _diceView;
        private readonly DiceContainerPresenter _diceContainer;
        private readonly RectTransform _startPoint;
        private readonly RectTransform _gameArea;

        public DicePresenter(Dice dice, DiceView diceView, DiceContainerPresenter diceContainer, RectTransform gameArea,
            RectTransform startPoint)
        {
            _dice = dice;
            _diceView = diceView;
            _diceContainer = diceContainer;
            _gameArea = gameArea;
            _startPoint = startPoint;
            _diceView.Clicked += OnClicked;
        }

        public void Dispose()
        {
            _diceView.Clicked -= OnClicked;
            _diceView?.Dispose();
        }

        public bool IsDiceInHand => _dice.IsInHand;
        public UnitType CurrentFaceTarget => _dice.CurrentFace.Config.Target;
        public IFace CurrentFace => _dice.CurrentFace;

        public void Roll()
        {
            _dice.SetRandomFace();
            _diceView.UpdateName(_dice.CurrentFace.GetType().Name);
            _diceView.PlayRollAnimation(_gameArea, _dice.Faces,
                () => _diceView.SetFaceIcon(_dice.CurrentFace.Config.Icon));
        }

        public void ApplyFace(IUnit unit, params float[] modifier)
        {
            _dice.CurrentFace.Apply(unit, modifier);
        }

        private void OnClicked()
        {
            if (_dice.IsInHand == false)
            {
                Take();
            }
            else
            {
                Throw(_startPoint);
            }
        }

        private void Take()
        {
            if (_dice.IsInHand)
            {
                return;
            }

            if (_diceContainer.IsFull)
            {
                return;
            }

            _dice.TakeInHand();
            _diceContainer.Add(_diceView.RectTransform);
            _diceView.DisableRigidbody();
        }

        public void Throw(RectTransform startPoint)
        {
            if (_dice.IsInHand == false)
            {
                return;
            }

            _dice.PutOnTable();
            _diceContainer.Remove(_diceView.RectTransform);
            _diceView.RectTransform.SetParent(_gameArea);
            _diceView.RectTransform.position = _startPoint.position;
            _diceView.EnableRigidbody();
            _diceView.PlayThrowAnimation(startPoint);
        }
    }
}