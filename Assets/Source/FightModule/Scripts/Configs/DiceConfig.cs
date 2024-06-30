using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.FaceSystem;
using Source.FightModule.Scripts.FaceSystem.Empty;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Source.FightModule.Scripts.Configs
{
    [CreateAssetMenu(menuName = Constants.Configs + nameof(DiceConfig), fileName = nameof(DiceConfig))]
    public class DiceConfig : SerializedScriptableObject
    {
        private const int MaxFaceCount = 6;

        [ValidateInput(nameof(IsValidFacesCount), "You must be add 6 faces!")]
        [ValidateInput(nameof(IsValidFaceConfig), "You must be set Config!")]
        [SerializeField]
        private List<IFace> _faces;

        [field: SerializeField] public Sprite Icon { get; private set; }

        public IReadOnlyList<IFace> Faces => _faces;

#if UNITY_EDITOR

        [Button]
        public void InitByDefault()
        {
            _faces?.Clear();
            _faces ??= new List<IFace>();

            const string emptyFaceConfigPath = "Assets/Source/FightModule/Configs/Faces/EmptyFaceConfig.asset";

            EmptyFaceConfig emptyFaceConfig;

            if (File.Exists(emptyFaceConfigPath))
            {
                emptyFaceConfig = AssetDatabase.LoadAssetAtPath<EmptyFaceConfig>(emptyFaceConfigPath);
            }
            else
            {
                emptyFaceConfig = CreateInstance<EmptyFaceConfig>();
                AssetDatabase.CreateAsset(emptyFaceConfig, emptyFaceConfigPath);
                AssetDatabase.SaveAssets();
            }

            for (int i = 0; i < MaxFaceCount; i++)
            {
                EmptyFace emptyFace = new();
                emptyFace.SetConfig(emptyFaceConfig);
                _faces.Add(emptyFace);
            }
        }

#endif

        [Button]
        private void ClearFaces()
        {
            _faces?.Clear();
        }

        private bool IsValidFacesCount()
        {
            return _faces.Count == 6 && _faces != null;
        }

        private bool IsValidFaceConfig()
        {
            return _faces.All(face => face.Config != null);
        }

        [Button]
        private void Init(IFace face, FaceConfig faceConfig)
        {
            _faces?.Clear();
            _faces ??= new List<IFace>();

            for (int i = 0; i < MaxFaceCount; i++)
            {
                object instance = Activator.CreateInstance(face.GetType());
                _faces.Add((IFace)instance);
                _faces[i].Config = faceConfig;
            }
        }
    }
}