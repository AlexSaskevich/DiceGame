using System;
using Source.FightModule.Scripts.StatSystem.Health;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem.Heal
{
    [Serializable]
    public class HealFace : IFace
    {
        [field: SerializeField] public FaceConfig Config { get; set; }

        public void Apply(IUnit unit, params float[] modifiers)
        {
            unit.ComponentContainer.GetComponent<HealthPresenter>().AddValue(Config.Value);
        }
    }
}