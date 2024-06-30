using System;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.FaceSystem;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Source.FightModule.Scripts.DiceSystem.Animation
{
    [Serializable]
    public class DiceAnimation
    {
        [field: SerializeField] public ThrowAnimationParameters ThrowParameters { get; private set; }
        [field: SerializeField] public RollAnimationParameters RollParameters { get; private set; }

        [Button]
        public void PlayThrowAnimation(Rigidbody2D rigidbody2D, RectTransform startPoint)
        {
            ResetRigidbody(rigidbody2D);
            rigidbody2D.position = startPoint.position;

            float angle = Random.Range(ThrowParameters.Angle.x, ThrowParameters.Angle.y) * Mathf.Deg2Rad;

            float forceX = Mathf.Cos(angle) *
                           Random.Range(ThrowParameters.ForceParameters.ThrowForce.x,
                               ThrowParameters.ForceParameters.ThrowForce.y);
            float forceY = Mathf.Sin(angle) *
                           Random.Range(ThrowParameters.ForceParameters.ThrowForce.x,
                               ThrowParameters.ForceParameters.ThrowForce.y);

            rigidbody2D.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            rigidbody2D.AddTorque(
                Random.Range(ThrowParameters.ForceParameters.TorqueForce.x,
                    ThrowParameters.ForceParameters.TorqueForce.y),
                ForceMode2D.Impulse);
        }

        [Button]
        public void PlayRollAnimation(Rigidbody2D rigidbody2D, RectTransform center, IReadOnlyList<IFace> faces,
            Image diceIcon, Action finished = null)
        {
            Vector2 randomPosition = (Vector2)center.position + Random.insideUnitCircle * RollParameters.Radius;

            ResetRigidbody(rigidbody2D);

            Vector2 direction = randomPosition - (Vector2)rigidbody2D.transform.position;
            direction.Normalize();

            ForceParameters forceParameters = RollParameters.ForceParameters;

            rigidbody2D.AddForce(direction * Random.Range(forceParameters.ThrowForce.x, forceParameters.ThrowForce.y),
                ForceMode2D.Impulse);
            rigidbody2D.AddTorque(Random.Range(forceParameters.TorqueForce.x, forceParameters.TorqueForce.y),
                ForceMode2D.Impulse);

            Sequence sequence = DOTween.Sequence();

            sequence.Join(rigidbody2D.transform
                .DOScale(Vector3.one * RollParameters.ScaleModifier, RollParameters.ScaleInFlyingDuration)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    int randomIndex = Random.Range(0, faces.Count);
                    diceIcon.sprite = faces[randomIndex].Config.Icon;
                }));

            sequence.Append(rigidbody2D.transform
                .DOScale(Vector3.one, RollParameters.ScaleInLandingDuration)
                .SetEase(Ease.OutBounce));

            int currentIndex = 0;

            sequence.Join(
                DOTween.To(() => currentIndex, x => currentIndex = x, faces.Count - 1,
                        duration: RollParameters.ScaleInLandingDuration)
                    .SetEase(Ease.OutBounce)
                    .OnUpdate(() => diceIcon.sprite = faces[currentIndex].Config.Icon)
                    .OnStart(() => Debug.LogError("Start!")));

            sequence.OnComplete(() => finished?.Invoke());
        }

        [Button]
        public void ResetRigidbody(Rigidbody2D rigidbody2D)
        {
            rigidbody2D.velocity = Vector2.zero;
            rigidbody2D.angularVelocity = 0f;
        }

        public void Draw()
        {
            RectTransform center = RollParameters.Center;

            if (center == null)
            {
                return;
            }

            Gizmos.DrawWireSphere(center.position, RollParameters.Radius);
        }
    }
}