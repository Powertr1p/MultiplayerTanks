using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof (LobbyViewAnimation))]
    public class LobbyView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _refreshButton;
        
        public event Action OnRefreshButtonClicked;
        
        private LobbyViewAnimation _animation;

        private Vector2 _initPosition;

        private void Awake()
        {
            _animation = GetComponent<LobbyViewAnimation>();
            _initPosition = transform.localPosition;
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(Hide);
            _refreshButton.onClick.AddListener(RefreshClicked);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Hide);
            _refreshButton.onClick.RemoveListener(RefreshClicked);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            transform.localPosition = _initPosition;
            
            _animation.PerformShowAnimation();
        }

        private void Hide()
        {
            _animation.PerformHideAnimation(() =>
            {
                gameObject.SetActive(false);
            });
        }

        private void RefreshClicked()
        {
            OnRefreshButtonClicked?.Invoke();
        }
    }
}