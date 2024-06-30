using UnityEngine;

namespace Source.FightModule.Scripts.StatSystem.Health
{
    public class Health : BaseStat
    {
        public Health(float currentValue, float defaultValue) : base(currentValue, defaultValue)
        {
        }

        public override void OnAddValue()
        {
        }

        public override void OnRemoveValue()
        {
            if (CurrentValue <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.LogError("Dead");
        }
    }
}