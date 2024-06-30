using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.FightModule.Scripts.UnitSystem.Configs
{
    [CreateAssetMenu(menuName = Constants.UnitsConfigs + nameof(PlayerConfig), fileName = nameof(PlayerConfig))]
    public class PlayerConfig : UnitConfig
    {
        [field: SerializeField, MinValue(1)] public int PlayerDiceCount { get; private set; } = 5;
        [field: SerializeField, MinValue(1)] public float DefaultEnergy { get; private set; } = 100;
    }
}