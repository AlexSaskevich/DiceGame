using Random = UnityEngine.Random;

namespace Source.FightModule.Scripts.DiceSystem
{
    public class DiceRoller
    {
        private const int MinValue = 0;
        private const int MaxFaceCount = 6;

        public DiceRoller(int maxRollCount)
        {
            MaxRollCount = maxRollCount;
        }

        public int CurrentRollCount { get; private set; }
        public int MaxRollCount { get; private set; }
        public bool CanRollDice => CurrentRollCount < MaxRollCount;

        public int GetRandomFaceIndex()
        {
            return Random.Range(MinValue, MaxFaceCount);
        }

        public void IncreaseRollCount()
        {
            CurrentRollCount++;
        }

        public void Reset()
        {
            CurrentRollCount = MinValue;
        }
    }
}