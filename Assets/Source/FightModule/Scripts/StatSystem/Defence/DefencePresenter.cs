using UnityEngine;

namespace Source.FightModule.Scripts.StatSystem.Defence
{
    public class DefencePresenter : StatPresenter
    {
        public DefencePresenter(IStat stat, StatView statView) : base(stat, statView)
        {
        }

        public float BlockDamage(float damage)
        {
            float defence = CurrentStatValue;
            float blockedDamage = Mathf.Min(defence, damage);
            float unblockedDamage = damage - blockedDamage;

            RemoveValue(blockedDamage);
            return unblockedDamage;
        }
    }
}