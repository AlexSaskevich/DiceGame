using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using Source.FightModule.Scripts.UnitSystem;
using Source.Utils.Timers;
using UnityEngine;

namespace Source.FightModule.Scripts.AbilitySystem
{
    public class AbilityPresenter : IDisposable
    {
        private readonly Ability _ability;
        private readonly AbilityView _view;
        private readonly Dictionary<Type, IUnit> _units;

        private Action _viewClicked;
        private readonly Dictionary<AbilityStatus, CountdownTimer> _countdownTimers;

        public AbilityPresenter(Ability ability, AbilityView view, Dictionary<Type, IUnit> units)
        {
            _ability = ability;
            _view = view;
            _units = units;
            _view.Clicked += OnViewClicked;
            _ability.SetStatus(AbilityStatus.Ready);

            _countdownTimers = new Dictionary<AbilityStatus, CountdownTimer>
            {
                { AbilityStatus.Cooldown, new CountdownTimer(_ability.Cooldown, OnCooldownFinished) },
                { AbilityStatus.Applying, new CountdownTimer(_ability.Duration, OnDurationFinished) }
            };
        }

        public void Dispose()
        {
            _view.Clicked -= OnViewClicked;
            _view?.Dispose();
        }

        public HashSet<AbilityStatus> CurrentStatuses => _ability.CurrentStatuses;
        public bool IsDelayed => _ability.IsDelayed;
        public string Name => _ability.name;
        public bool CanCanceled => _ability.CanCanceled;

        public void UpdateTimers()
        {
            _countdownTimers.ForEach(x => x.Value.Tick(1));
            _view.SetCooldown((int)_countdownTimers[AbilityStatus.Cooldown].Time);
            _view.SetDuration((int)_countdownTimers[AbilityStatus.Applying].Time);
        }

        public void ApplyEffects(IUnit caster, IUnit target)
        {
            _ability.ApplyEffects(caster, target);
        }

        public void Use(IUnit caster, IUnit target)
        {
            if (_ability.IsDelayed == false)
            {
                _ability.Use(caster, target);
            }
            else
            {
                _ability.UseDelayed(caster, target);
            }

            if (CurrentStatuses.Contains(AbilityStatus.Ready) && CurrentStatuses.Contains(AbilityStatus.Applying))
            {
                CountdownTimer durationTimer = _countdownTimers[AbilityStatus.Applying];
                durationTimer.Reset();
                durationTimer.Run();
                _view.SetDuration((int)durationTimer.Time);
                _countdownTimers[AbilityStatus.Cooldown].Run();
            }
            else if (CurrentStatuses.Contains(AbilityStatus.Cooldown) &&
                     CurrentStatuses.Contains(AbilityStatus.Applying))
            {
                _countdownTimers.ForEach(x => x.Value.Run());
            }
            else
            {
                _countdownTimers[AbilityStatus.Cooldown].Run();
            }
        }

        private void OnViewClicked()
        {
            Use(_units[typeof(Player)], _units[typeof(Enemy)]);
        }

        private void OnCooldownFinished()
        {
            Debug.LogError($"Cooldown finished! - {Name}");
            _countdownTimers[AbilityStatus.Cooldown].Reset();
            _ability.RemoveStatus(AbilityStatus.Cooldown);
            _ability.AddStatus(AbilityStatus.Ready);
        }

        private void OnDurationFinished()
        {
            Debug.LogError($"Duration finished! - {Name}");
            _countdownTimers[AbilityStatus.Applying].Reset();
            _ability.RemoveStatus(AbilityStatus.Applying);
        }

        public void Cancel()
        {
            Debug.LogError($"Ability {Name} canceled!");
            CountdownTimer countdownTimer = _countdownTimers[AbilityStatus.Applying];
            countdownTimer.Reset();
            _ability.RemoveStatus(AbilityStatus.Applying);
            _view.SetDuration((int)countdownTimer.Time);
        }
    }
}