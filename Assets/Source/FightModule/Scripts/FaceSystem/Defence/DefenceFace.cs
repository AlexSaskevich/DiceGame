using System;
using Source.FightModule.Scripts.StatSystem.Defence;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem.Defence
{
    [Serializable]
    public class DefenceFace : IFace
    {
        [field: SerializeField] public FaceConfig Config { get; set; }


        public void Apply(IUnit unit, params float[] modifiers)
        {
            Debug.Log($"{nameof(DefenceFace)}\n{nameof(Config.Value)} = {Config.Value}");
            unit.ComponentContainer.GetComponent<DefencePresenter>().AddValue(Config.Value);
        }
    }
}