using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using Source.FightModule.Scripts.DiceSystem.Animation;
using Source.FightModule.Scripts.FaceSystem;
using UnityEngine;
using UnityEngine.UI;

namespace Source.FightModule.Scripts.DiceSystem
{
    public class DiceView : MonoBehaviour, IDisposable
    {
        [SerializeField] private Image _diceIcon;
        [SerializeField] private Image _faceIcon;
        [SerializeField] private Button _button;
        [SerializeField] private DiceAnimation _animation;
        [SerializeField] private Rigidbody2D _rigidbody2D;

        public RectTransform RectTransform => transform as RectTransform;

        public event Action Clicked;

        public void Init()
        {
            _button.onClick.AddListener(OnButtonClick);
        }

        public void Dispose()
        {
            _button.onClick.RemoveListener(OnButtonClick);
        }

        public void SetFaceIcon(Sprite icon)
        {
            _faceIcon.sprite = icon;
        }

        public void SetDiceIcon(Sprite icon)
        {
            _diceIcon.sprite = icon;
        }

        public void SetName(string diceName)
        {
            gameObject.name = diceName;
        }

        public void UpdateName(string diceName)
        {
            string currentName = gameObject.name;
            string[] nameParts = currentName.Split('-');
            string newName = $"{nameParts[0]}-{diceName}";
            gameObject.name = newName;
        }

        public void PlayThrowAnimation(RectTransform startPoint)
        {
            _animation.PlayThrowAnimation(_rigidbody2D, startPoint);
        }

        public void PlayRollAnimation(RectTransform center, IReadOnlyList<IFace> faces, Action finished = null)
        {
            _animation.PlayRollAnimation(_rigidbody2D, center, faces, _faceIcon, finished);
        }

        public async void StartAnimateFaces(IReadOnlyList<IFace> faces)
        {
            using IEnumerator<IFace> enumerator = faces.GetEnumerator();
            CancellationTokenSource cancellationTokenSource = new();

            while (true)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    cancellationTokenSource.Cancel();
                    break;
                }

                if (enumerator.MoveNext() == false)
                {
                    enumerator.Reset();
                    continue;
                }

                IFace face = enumerator.Current;
                SetFaceIcon(face.Config.Icon);
                await Task.Delay(TimeSpan.FromSeconds(0.1f));
            }
        }

        [Button]
        public void DisableRigidbody()
        {
            _animation.ResetRigidbody(_rigidbody2D);
            _rigidbody2D.simulated = false;
            RectTransform.rotation = Quaternion.identity;
        }

        [Button]
        public void EnableRigidbody()
        {
            _rigidbody2D.simulated = true;
        }

        private void OnButtonClick()
        {
            Clicked?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            DrawAngleGizmos();

            Gizmos.color = Color.yellow;
            _animation.Draw();
        }

        private void DrawAngleGizmos()
        {
            if (_animation.ThrowParameters.StartPoint == null)
            {
                return;
            }

            const float rayLength = 20f;

            Vector3 minAngleRay =
                Quaternion.Euler(0f, 0f, _animation.ThrowParameters.Angle.x) * Vector3.right * rayLength;
            Gizmos.color = Color.green;
            Gizmos.DrawRay(_animation.ThrowParameters.StartPoint.position, minAngleRay);

            Vector3 maxAngleRay = Quaternion.Euler(0f, 0f, _animation.ThrowParameters.Angle.y) * Vector3.right *
                                  rayLength;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_animation.ThrowParameters.StartPoint.position, maxAngleRay);
        }
    }
}