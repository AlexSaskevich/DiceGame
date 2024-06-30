using System;
using System.Collections.Generic;
using Source.FightModule.Scripts.FaceSystem;

namespace Source.FightModule.Scripts.DiceSystem
{
    public class Dice
    {
        public Dice(IReadOnlyList<IFace> faces, DiceRoller roller)
        {
            Faces = faces;
            Roller = roller;
        }

        public event Action FaceChanged;

        public IReadOnlyList<IFace> Faces { get; }
        public DiceRoller Roller { get; }
        public IFace CurrentFace { get; private set; }
        public bool IsInHand { get; private set; }

        public void SetFace(IFace face)
        {
            CurrentFace = face;
            FaceChanged?.Invoke();
        }

        public void SetRandomFace()
        {
            int randomFaceIndex = Roller.GetRandomFaceIndex();
            IFace face = Faces[randomFaceIndex];
            CurrentFace = face;
            FaceChanged?.Invoke();
        }

        public void PutOnTable()
        {
            IsInHand = false;
        }

        public void TakeInHand()
        {
            IsInHand = true;
        }
    }
}