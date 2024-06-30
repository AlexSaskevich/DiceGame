using TMPro;
using UnityEngine;

namespace Source.FightModule.Scripts.DiceSystem
{
    public class RollCountView : MonoBehaviour
    {
        [field: SerializeField] public TextMeshProUGUI RollCountText { get; private set; }

        public void Set(int currentRollCount, int maxRollCount)
        {
            RollCountText.text = $"Rolls: {currentRollCount}/{maxRollCount}";
        }
    }
}