namespace Source.FightModule.Scripts.StatSystem.Health
{
    public class HealthPresenter : StatPresenter
    {
        private readonly Health _health;

        public HealthPresenter(IStat stat, StatView statView) : base(stat, statView)
        {
            _health = (Health)stat;
        }

        public bool IsAlive => _health.CurrentValue > 0;
    }
}