using System;
using System.Linq;
using Source.FightModule.Scripts.StatSystem.Health;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem.Attack
{
    [Serializable]
    public class AttackFace : IFace
    {
        [field: SerializeField] public FaceConfig Config { get; set; }

        public void Apply(IUnit unit, params float[] modifiers)
        {
            float finalDamage = modifiers.Aggregate(Config.Value, (current, modifier) => current * modifier);

            unit.ComponentContainer.GetComponent<HealthPresenter>().RemoveValue(finalDamage);
        }
    }
}