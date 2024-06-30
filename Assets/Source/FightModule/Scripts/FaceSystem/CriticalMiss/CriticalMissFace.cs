using System;
using Source.FightModule.Scripts.StatSystem.Health;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem.CriticalMiss
{
    [Serializable]
    public class CriticalMissFace : IFace
    {
        [field: SerializeField] public FaceConfig Config { get; set; }


        public void Apply(IUnit unit, params float[] modifiers)
        {
            unit.ComponentContainer.GetComponent<HealthPresenter>().RemoveValue(Config.Value);
        }
    }
}