using System.Collections.Generic;
using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem.Container
{
    public class DiceContainer
    {
        private readonly List<RectTransform> _views = new();

        public DiceContainer(int capacity)
        {
            Capacity = capacity;
        }

        public int Capacity { get; }
        public bool IsFull => _views.Count == Capacity - 1;
        public int Count => _views.Count;

        public void Add(RectTransform diceView)
        {
            _views.Add(diceView);
        }

        public void Remove(RectTransform diceView)
        {
            _views.Remove(diceView);
        }
    }
}