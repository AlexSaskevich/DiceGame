using TMPro;
using UnityEngine;

namespace Source.FightModule.Scripts.UnitSystem
{
    public abstract class UnitView : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI Name { get; private set; }
    }
}