using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof (LobbyViewAnimation))]
    public class LobbyView : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        
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
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(Hide);
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
    }
}