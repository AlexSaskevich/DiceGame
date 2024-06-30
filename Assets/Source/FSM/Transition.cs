using System;
using System.Threading.Tasks;

namespace Source.FSM
{
    public class Transition<T>
    {
        public Transition(IState<T> from, IState<T> to, Func<bool> condition)
        {
            From = from;
            To = to;
            Condition = condition;
        }

        public IState<T> From { get; private set; }
        public IState<T> To { get; private set; }
        public Func<bool> Condition { get; private set; }
    }
}