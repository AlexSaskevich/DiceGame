using System;

namespace Source.Utils.Timers
{
    public class CountdownTimer : Timer
    {
        private readonly Action _onFinished;

        public CountdownTimer(float value, Action onFinished = null) : base(value)
        {
            _onFinished = onFinished;
        }

        public override void Tick(float deltaTime)
        {
            if (IsRunning && Time > 0)
            {
                Time -= deltaTime;
            }

            if (IsRunning && Time <= 0)
            {
                Stop(_onFinished);
            }
        }

        public void Reset()
        {
            Time = InitialTime;
        }
    }
}