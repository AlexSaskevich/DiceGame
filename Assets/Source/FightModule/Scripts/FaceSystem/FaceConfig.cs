using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem
{
    public abstract class FaceConfig : ScriptableObject
    {
        [field: SerializeField, MinValue(0)] public float Value { get; private set; } = 1;
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public UnitType Target { get; private set; }
    }

    [Serializable]
    public struct UnitType
    {
        [ValueDropdown(nameof(UnitTypes)), HideLabel]
        public string Value;

        private List<string> UnitTypes =>
            Assembly.GetAssembly(typeof(IUnit)).GetTypes()
                .Where(x => x.IsClass && x.GetInterface(nameof(IUnit)) != null)
                .Select(x => x.Name).ToList();
    }
}