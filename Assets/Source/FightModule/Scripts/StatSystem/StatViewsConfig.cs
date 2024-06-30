using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Source.FightModule.Scripts.StatSystem
{
    [CreateAssetMenu(menuName = Constants.Configs + nameof(StatViewsConfig), fileName = nameof(StatViewsConfig))]
    public class StatViewsConfig : SerializedScriptableObject
    {
        [SerializeField] private Dictionary<StatViewType, StatView> _views;

        public StatView GetViewForType<T>()
        {
            KeyValuePair<StatViewType, StatView> matchingKeyValuePair = _views.FirstOrDefault(keyValuePair =>
                keyValuePair.Key.Value == typeof(T).Name);

            if (matchingKeyValuePair.Equals(default(KeyValuePair<StatViewType, StatView>)) == false)
            {
                return matchingKeyValuePair.Value;
            }

            Debug.LogError($"In {nameof(StatViewsConfig)} dictionary doesn't contain key {typeof(T).Name}");
            return null;
        }

        [Serializable]
        public struct StatViewType
        {
            [ValueDropdown(nameof(ViewTypes)), HideLabel]
            public string Value;

            private List<string> ViewTypes =>
                Assembly.GetAssembly(typeof(BaseStat)).GetTypes()
                    .Where(x => x.IsClass && x.GetInterface(nameof(IStat)) != null)
                    .Select(x => x.Name).ToList();
        }
    }
}