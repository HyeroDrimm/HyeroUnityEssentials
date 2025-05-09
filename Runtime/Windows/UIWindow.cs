using System.Collections;
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
        [SerializeField] private AnimationType _showAnimation = AnimationType.Fade;
        [SerializeField] private AnimationType _hideAnimation = AnimationType.Fade;

        [Header("Events")]
        [SerializeField] private UnityEvent _onShowBegin;
        [SerializeField] private UnityEvent _onShowComplete;
        [SerializeField] private UnityEvent _onHideBegin;
        [SerializeField] private UnityEvent _onHideComplete;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private bool _isAnimating = false;
        private Coroutine _animationCoroutine;

        public string WindowId => _windowId;
        public bool IsModal => _isModal;
        public bool IsVisible => _canvasGroup == null || _canvasGroup.alpha > 0;

        /// <summary>
        /// Animation types for showing/hiding windows
        /// </summary>
        public enum AnimationType
        {
            None,
            Fade,
            SlideFromRight,
            SlideFromLeft,
            SlideFromTop,
            SlideFromBottom,
            Scale,
            Custom
        }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            _rectTransform = GetComponent<RectTransform>();

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
            SetupInitialShowState();

            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                float t = elapsed / _animationDuration;

                // Apply animation based on type
                ApplyShowAnimation(t);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // Ensure final state is correct
            ApplyShowAnimation(1f);

            _isAnimating = false;

            // Invoke show complete event
            _onShowComplete?.Invoke();
        }

        protected virtual IEnumerator AnimateHide()
        {
            _isAnimating = true;

            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                float t = elapsed / _animationDuration;

                // Apply animation based on type
                ApplyHideAnimation(t);

                elapsed += Time.unscaledDeltaTime;
                yield return null;
            }

            // Ensure final state is correct
            ApplyHideAnimation(1f);

            _isAnimating = false;

            // Invoke hide complete event
            _onHideComplete?.Invoke();
        }

        protected virtual void SetupInitialShowState()
        {
            switch (_showAnimation)
            {
                case AnimationType.None:
                    _canvasGroup.alpha = 1f;
                    break;

                case AnimationType.Fade:
                    _canvasGroup.alpha = 0f;
                    break;

                case AnimationType.SlideFromRight:
                    _canvasGroup.alpha = 1f;
                    _rectTransform.anchoredPosition = new Vector2(Screen.width, _rectTransform.anchoredPosition.y);
                    break;

                case AnimationType.SlideFromLeft:
                    _canvasGroup.alpha = 1f;
                    _rectTransform.anchoredPosition = new Vector2(-Screen.width, _rectTransform.anchoredPosition.y);
                    break;

                case AnimationType.SlideFromTop:
                    _canvasGroup.alpha = 1f;
                    _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, Screen.height);
                    break;

                case AnimationType.SlideFromBottom:
                    _canvasGroup.alpha = 1f;
                    _rectTransform.anchoredPosition = new Vector2(_rectTransform.anchoredPosition.x, -Screen.height);
                    break;

                case AnimationType.Scale:
                    _canvasGroup.alpha = 1f;
                    _rectTransform.localScale = Vector3.zero;
                    break;

                case AnimationType.Custom:
                    // Override in child class
                    break;
            }
        }

        protected virtual void ApplyShowAnimation(float t)
        {
            // Apply easing
            float easedT = EaseInOut(t);

            switch (_showAnimation)
            {
                case AnimationType.None:
                    // No animation
                    break;

                case AnimationType.Fade:
                    _canvasGroup.alpha = easedT;
                    break;

                case AnimationType.SlideFromRight:
                    Vector2 startPos = new Vector2(Screen.width, _rectTransform.anchoredPosition.y);
                    Vector2 endPos = new Vector2(0, _rectTransform.anchoredPosition.y);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.SlideFromLeft:
                    startPos = new Vector2(-Screen.width, _rectTransform.anchoredPosition.y);
                    endPos = new Vector2(0, _rectTransform.anchoredPosition.y);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.SlideFromTop:
                    startPos = new Vector2(_rectTransform.anchoredPosition.x, Screen.height);
                    endPos = new Vector2(_rectTransform.anchoredPosition.x, 0);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.SlideFromBottom:
                    startPos = new Vector2(_rectTransform.anchoredPosition.x, -Screen.height);
                    endPos = new Vector2(_rectTransform.anchoredPosition.x, 0);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.Scale:
                    _rectTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, easedT);
                    break;

                case AnimationType.Custom:
                    // Override in child class
                    break;
            }
        }

        protected virtual void ApplyHideAnimation(float t)
        {
            // Apply easing
            float easedT = EaseInOut(t);

            switch (_hideAnimation)
            {
                case AnimationType.None:
                    // No animation, just set alpha to 0 at the end
                    _canvasGroup.alpha = t >= 1f ? 0f : 1f;
                    break;

                case AnimationType.Fade:
                    _canvasGroup.alpha = 1f - easedT;
                    break;

                case AnimationType.SlideFromRight:
                    Vector2 startPos = new Vector2(0, _rectTransform.anchoredPosition.y);
                    Vector2 endPos = new Vector2(-Screen.width, _rectTransform.anchoredPosition.y);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.SlideFromLeft:
                    startPos = new Vector2(0, _rectTransform.anchoredPosition.y);
                    endPos = new Vector2(Screen.width, _rectTransform.anchoredPosition.y);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.SlideFromTop:
                    startPos = new Vector2(_rectTransform.anchoredPosition.x, 0);
                    endPos = new Vector2(_rectTransform.anchoredPosition.x, -Screen.height);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.SlideFromBottom:
                    startPos = new Vector2(_rectTransform.anchoredPosition.x, 0);
                    endPos = new Vector2(_rectTransform.anchoredPosition.x, Screen.height);
                    _rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
                    break;

                case AnimationType.Scale:
                    _rectTransform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, easedT);
                    break;

                case AnimationType.Custom:
                    // Override in child class
                    break;
            }
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