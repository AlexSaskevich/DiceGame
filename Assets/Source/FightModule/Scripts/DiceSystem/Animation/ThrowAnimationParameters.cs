using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem.Animation
{
    [Serializable]
    public struct ThrowAnimationParameters
    {
        [field: SerializeField] public Transform StartPoint { get; private set; }
        [field: SerializeField] public ForceParameters ForceParameters { get; private set; }

        [field: SerializeField, MinMaxSlider(0, 180, true)]
        public Vector2 Angle { get; private set; }
    }
}