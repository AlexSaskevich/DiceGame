using UnityEngine;
using UnityEngine.UI;

namespace Source.FightModule.Scripts.DiceSystem.Container
{
    public class DiceContainerView : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [field: SerializeField] public HorizontalLayoutGroup HorizontalLayoutGroup { get; private set; }

        public RectTransform SelfRectTransform => (RectTransform)transform;

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}