using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem.Container
{
    public class DiceContainerPresenter
    {
        private readonly DiceContainer _diceContainer;
        private readonly DiceContainerView _view;

        public DiceContainerPresenter(DiceContainer diceContainer, DiceContainerView view)
        {
            _diceContainer = diceContainer;
            _view = view;
        }

        public RectTransform RectTransform => _view.SelfRectTransform;
        public bool IsFull => _diceContainer.IsFull;

        public void Show()
        {
            _view.Show();
        }

        public void Hide()
        {
            _view.Hide();
        }

        public void Add(RectTransform diceView)
        {
            _diceContainer.Add(diceView);
            diceView.SetParent(_view.HorizontalLayoutGroup.transform);


            if (_diceContainer.Count > 0)
            {
                Show();
            }
        }

        public void Remove(RectTransform diceView)
        {
            _diceContainer.Remove(diceView);

            if (_diceContainer.Count == 0)
            {
                Hide();
            }
        }
    }
}