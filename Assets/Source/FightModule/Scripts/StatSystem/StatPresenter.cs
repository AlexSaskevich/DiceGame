namespace Source.FightModule.Scripts.StatSystem
{
    public abstract class StatPresenter
    {
        private readonly IStat _stat;
        private readonly StatView _statView;

        protected StatPresenter(IStat stat, StatView statView)
        {
            _stat = stat;
            _statView = statView;
            _stat.Changed += OnChanged;
        }

        public float CurrentStatValue => _stat.CurrentValue;

        public void Dispose()
        {
            _stat.Changed -= OnChanged;
        }

        public void AddValue(float value)
        {
            _stat.AddValue(value);
        }

        public void RemoveValue(float value)
        {
            _stat.RemoveValue(value);
        }

        private void OnChanged(float currentHealth)
        {
            _statView.Refresh(currentHealth);
        }
    }
}