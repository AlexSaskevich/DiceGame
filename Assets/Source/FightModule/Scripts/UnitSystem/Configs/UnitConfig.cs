using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.FightModule.Scripts.UnitSystem.Configs
{
    public abstract class UnitConfig : ScriptableObject
    {
        [field: SerializeField, MinValue(0)] public float DefaultHealth { get; private set; } = 100f;
        [field: SerializeField, MinValue(0)] public float MinHealth { get; private set; }
        [field: SerializeField, MinValue(0)] public float DefaultDefence { get; private set; }
    }
}