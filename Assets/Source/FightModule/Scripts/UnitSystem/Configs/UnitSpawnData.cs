using System;
using Source.FightModule.Scripts.StatSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.UnitSystem.Configs
{
    [Serializable]
    public struct UnitSpawnData
    {
        [field: SerializeField] public UnitConfig Config { get; private set; }
        [field: SerializeField] public UnitView UnitView { get; private set; }
        [field: SerializeField] public RectTransform Parent { get; private set; }
        [field: SerializeField] public StatViewsConfig StatViewsConfig { get; private set; }
        [field: SerializeField] public RectTransform AbilityViewsContainer { get; private set; }
    }
}