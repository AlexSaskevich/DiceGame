using System;
using Source.FightModule.Scripts.StatSystem.Energy;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem.Energy
{
    [Serializable]
    public class EnergyFace : IFace
    {
        [field: SerializeField] public FaceConfig Config { get; set; }


        public void Apply(IUnit unit, params float[] modifiers)
        {
            Debug.Log($"{GetType().Name}\n{nameof(Config.Value)} = {Config.Value}");
            unit.ComponentContainer.GetComponent<EnergyPresenter>().AddValue(Config.Value);
        }
    }
}