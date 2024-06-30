using System;
using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem.Animation
{
    [Serializable]
    public struct RollAnimationParameters
    {
        [field: SerializeField] public RectTransform Center { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float ScaleModifier { get; private set; }
        [field: SerializeField] public float ScaleInFlyingDuration { get; private set; }
        [field: SerializeField] public float ScaleInLandingDuration { get; private set; }
        [field: SerializeField] public ForceParameters ForceParameters { get; private set; }
    }
}