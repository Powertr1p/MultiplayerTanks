using System;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class LobbyViewAnimation : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.7f;
        [SerializeField] private Ease _showEasing = Ease.OutBounce;
        [SerializeField] private Ease _hideEasing = Ease.InExpo;

        private Transform _transform;
        private float _hideYposition;
        private float _showYposition = 0;

        private void Awake()
        {
            _transform = transform;
            _hideYposition = _transform.localPosition.y;
        }

        public void PerformShowAnimation(Action completeCallback = null)
        {
            _transform.DOLocalMoveY(_showYposition, _duration).SetEase(_showEasing).OnComplete(() =>
            {
                completeCallback?.Invoke();
            });
        }

        public void PerformHideAnimation(Action completeCallback = null)
        {
            _transform.DOLocalMoveY(_hideYposition, _duration / 2).SetEase(_hideEasing).OnComplete(() =>
            {
                completeCallback?.Invoke();
            });
        }
    }
}