using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem.Animation
{
    [Serializable]
    public struct ForceParameters
    {
        [field: SerializeField, MinMaxSlider(0f, 30f, true)]
        public Vector2 ThrowForce { get; private set; }

        [field: SerializeField, MinMaxSlider(0, 1f, true)]
        public Vector2 TorqueForce { get; private set; }
    }
}