using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.FightModule.Scripts.AbilitySystem
{
    public class AbilityView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private TextMeshProUGUI _cooldown;
        [SerializeField] private TextMeshProUGUI _duration;
        [SerializeField] private Button _button;

        public event Action Clicked;

        public void Init()
        {
            _button.onClick.AddListener(OnButtonClicked);
        }

        public void Dispose()
        {
            _button.onClick.RemoveListener(OnButtonClicked);
        }

        public void Set(Sprite icon, string abilityName, string cost, int cooldown, int duration)
        {
            _icon.sprite = icon;
            _name.text = abilityName;
            _cost.text = cost;
            _cooldown.text = cooldown.ToString();
            _duration.text = duration > 0 ? duration.ToString() : string.Empty;
        }

        public void SetCooldown(int cooldown)
        {
            _cooldown.text = cooldown.ToString();
        }

        public void SetDuration(int duration)
        {
            _duration.text = duration > 0 ? duration.ToString() : string.Empty;
        }

        private void OnButtonClicked()
        {
            Clicked?.Invoke();
        }
    }
}