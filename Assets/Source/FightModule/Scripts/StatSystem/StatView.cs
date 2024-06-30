using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.FightModule.Scripts.StatSystem
{
    public class StatView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _healthText;
        [SerializeField] private Image _image;

        public void Refresh(float currentHealth)
        {
            _healthText.text = currentHealth.ToString();
            _image.fillAmount = Mathf.Clamp01(currentHealth / 100);
        }
    }
}