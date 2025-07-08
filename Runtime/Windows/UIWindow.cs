using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace HyeroUnityEssentials.WindowSystem
{
    /// <summary>
    /// Base class for all UI windows that provides common functionality
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public class UIWindow : MonoBehaviour
    {
        [Header("Window Settings")]
        [SerializeField] private string _windowId;
        [SerializeField] private bool _isModal = false;
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] protected RectTransform _rectTransform;

        [Header("Events")]
        [SerializeField] private UnityEvent _onShowBegin;
        [SerializeField] private UnityEvent _onShowComplete;
        [SerializeField] private UnityEvent _onHideBegin;
        [SerializeField] private UnityEvent _onHideComplete;

        protected CanvasGroup _canvasGroup;
        protected WindowAnimation _windowAnimation;
        private bool _isAnimating = false;
        private Coroutine _animationCoroutine;

        public string WindowId => _windowId;
        public bool IsModal => _isModal;
        public bool IsVisible => _canvasGroup == null || _canvasGroup.alpha > 0;

        protected virtual void Awake()
        {
            if (!gameObject.TryGetComponent(out _canvasGroup))
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            if (!_rectTransform)
                gameObject.TryGetComponent(out _rectTransform);

            // Auto-generate window ID if not set
            if (string.IsNullOrEmpty(_windowId))
            {
                _windowId = gameObject.name;
            }

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;
        }

        /// <summary>
        /// Show this window with animation
        /// </summary>
        [Button]
        public virtual void Show()
        {
            if (_isAnimating)
            {
                // Stop current animation if any
                if (_animationCoroutine != null)
                {
                    StopCoroutine(_animationCoroutine);
                }
            }

            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.alpha = 1;

            // Invoke show begin event
            _onShowBegin?.Invoke();

            // Start show animation
            _animationCoroutine = StartCoroutine(AnimateShow());
        }

        /// <summary>
        /// Hide this window with animation
        /// </summary>
        [Button]

        public virtual void Hide()
        {
            if (_isAnimating)
            {
                // Stop current animation if any
                if (_animationCoroutine != null)
                {
                    StopCoroutine(_animationCoroutine);
                }
            }

            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = 0;

            // Invoke hide begin event
            _onHideBegin?.Invoke();

            // Start hide animation
            _animationCoroutine = StartCoroutine(AnimateHide());
        }

        protected virtual IEnumerator AnimateShow()
        {
            _isAnimating = true;

            // Prepare initial state based on animation type
            _windowAnimation.SetupShow();

            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                float t = elapsed / _animationDuration;

                // Apply animation based on type
                _windowAnimation.DoShow(t);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // Ensure final state is correct
            _windowAnimation.DoShow(1f);

            _windowAnimation.EndShow();

            _isAnimating = false;

            // Invoke show complete event
            _onShowComplete?.Invoke();
        }

        protected virtual IEnumerator AnimateHide()
        {
            _isAnimating = true;

            _windowAnimation.SetupHide();

            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                float t = elapsed / _animationDuration;

                // Apply animation based on type
                _windowAnimation.DoHide(t);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // Ensure final state is correct
            _windowAnimation.DoHide(1);

            _windowAnimation.EndHide();

            _isAnimating = false;

            // Invoke hide complete event
            _onHideComplete?.Invoke();
        }

        protected virtual void ApplyHideAnimation(float t)
        {
            _windowAnimation.DoHide(t);
        }

        /// <summary>
        /// Simple ease in/out function for smooth animations
        /// </summary>
        protected float EaseInOut(float t)
        {
            return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
        }

        /// <summary>
        /// Close this window
        /// </summary>
        public void Close()
        {
            if (IsModal)
            {
                WindowManager.Instance.CloseModal();
            }
            else
            {
                WindowManager.Instance.Back();
            }
        }

        /// <summary>
        /// Switch to another window
        /// </summary>
        public void SwitchTo(string windowPrefabPath, WindowTransitionType transitionType = WindowTransitionType.Push)
        {
            WindowManager.Instance.Show(windowPrefabPath, transitionType);
        }

        /// <summary>
        /// Switch to another window
        /// </summary>
        public void SwitchTo(UIWindow window, WindowTransitionType transitionType = WindowTransitionType.Push)
        {
            WindowManager.Instance.Show(window, transitionType);
        }
    }
}