using System;

namespace Source.Utils.Timers
{
    public abstract class Timer
    {
        protected float InitialTime { get; }
        public float Time { get; set; }
        public bool IsRunning { get; private set; }

        protected Timer(float value)
        {
            InitialTime = value;
            IsRunning = false;
        }

        public void Run(Action onTimerRun = null)
        {
            if (IsRunning)
            {
                return;
            }

            Time = InitialTime;
            IsRunning = true;
            onTimerRun?.Invoke();
        }

        public void Stop(Action onTimerStop = null)
        {
            if (IsRunning == false)
            {
                return;
            }

            IsRunning = false;
            onTimerStop?.Invoke();
        }

        public abstract void Tick(float deltaTime);
    }
}