using System;
using Source.FightModule.Scripts.UnitSystem;
using UnityEngine;

namespace Source.FightModule.Scripts.FaceSystem.Empty
{
    [Serializable]
    public class EmptyFace : IFace
    {
        [field: SerializeField] public FaceConfig Config { get; set; }

        public void Apply(IUnit unit, params float[] modifiers)
        {
            Debug.Log("Empty!");
        }

        /// <summary>
        /// Editor only!
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void SetConfig(FaceConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Config = config;
        }
    }
}