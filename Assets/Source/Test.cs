using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace Source
{
    public class Test : SerializedMonoBehaviour
    {
        [SerializeField] private List<Sprite> _images;
        [SerializeField] private Image _view;
        [SerializeField] private float _animationDuration;
        [SerializeField] private Ease _ease = Ease.OutBounce;

        [Button]
        private void StartRoll()
        {
            int currentIndex = 0;

            DOTween.To(() => currentIndex, x => currentIndex = x, _images.Count - 1, _animationDuration)
                .SetEase(_ease)
                .OnUpdate(() => _view.sprite = _images[currentIndex]);
        }
    }
}