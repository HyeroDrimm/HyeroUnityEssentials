using System.Collections.Generic;
using UnityEngine;

namespace HyeroUnityEssentials.WindowSystem
{
    /// <summary>
    /// Window transition types define how a new window appears in relation to the current one
    /// </summary>
    public enum WindowTransitionType
    {
        /// <summary>Replace current window with the new one</summary>
        Replace,

        /// <summary>Show new window on top of current window (current window stays active)</summary>
        Stack,

        /// <summary>Show new window and add previous to back stack</summary>
        Push
    }

    /// <summary>
    /// Main window manager that handles all window-related operations
    /// </summary>
    [DefaultExecutionOrder(-11)]
    public class WindowManager : MonoBehaviour
    {
        private static WindowManager _instance;
        public static WindowManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    Debug.Log("No window manager");
                }
                return _instance;
            }
        }

        [SerializeField] private UIWindow _startingWindow;
        [SerializeField] private Transform _windowContainer;
        [SerializeField] private bool _allowMultipleModals = false;

        private UIWindow _currentWindow;
        private UIWindow _currentModal;
        private Stack<UIWindow> _windowHistory = new Stack<UIWindow>();
        private Dictionary<string, UIWindow> _cachedWindows = new Dictionary<string, UIWindow>();

        public bool IsHistoryEmpty => _windowHistory.Count == 0;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;

            // if (_windowContainer == null)
            // {
            //     GameObject container = new GameObject("WindowContainer");
            //     container.transform.SetParent(transform);
            //     _windowContainer = container.transform;
            // }

            // Initialize and show starting window if assigned
            if (_startingWindow != null)
            {
                Show(_startingWindow, WindowTransitionType.Replace);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Back();
            }
        }

        /// <summary>
        /// Show a window by reference
        /// </summary>
        public void Show(UIWindow window, WindowTransitionType transitionType = WindowTransitionType.Push)
        {
            if (window == null)
            {
                Debug.LogError("Window that was tried to be shown was null", this);
                return;
            }

            // Check if it's a modal window
            if (window.IsModal)
            {
                ShowModal(window);
                return;
            }

            switch (transitionType)
            {
                case WindowTransitionType.Replace:
                    ReplaceWindow(window);
                    break;
                case WindowTransitionType.Stack:
                    StackWindow(window);
                    break;
                case WindowTransitionType.Push:
                    PushWindow(window);
                    break;
            }
        }

        /// <summary>
        /// Show a window by prefab path
        /// </summary>
        public void Show(string prefabPath, WindowTransitionType transitionType = WindowTransitionType.Push)
        {
            // Try to get from cache first
            if (_cachedWindows.TryGetValue(prefabPath, out UIWindow window))
            {
                Show(window, transitionType);
                return;
            }

            // Load the window prefab
            UIWindow prefab = Resources.Load<UIWindow>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"Window prefab not found at path: {prefabPath}");
                return;
            }

            // Instantiate and cache the window
            window = Instantiate(prefab, _windowContainer);
            window.gameObject.SetActive(false);
            _cachedWindows[prefabPath] = window;

            Show(window, transitionType);
        }

        /// <summary>
        /// Show a modal window on top of current window
        /// </summary>
        public void ShowModal(UIWindow modalWindow)
        {
            if (modalWindow == null) return;

            // If another modal is already open and we don't allow multiple modals
            if (_currentModal != null && !_allowMultipleModals)
            {
                _currentModal.Hide();
                _currentModal = null;
            }

            _currentModal = modalWindow;
            modalWindow.transform.SetParent(_windowContainer);
            modalWindow.transform.SetAsLastSibling(); // Ensure it's on top

            // Show the modal
            modalWindow.Show();
        }

        /// <summary>
        /// Close the current modal window if any
        /// </summary>
        public void CloseModal()
        {
            if (_currentModal != null)
            {
                _currentModal.Hide();
                _currentModal = null;
            }
        }

        /// <summary>
        /// Navigate back to the previous window
        /// </summary>
        public void Back()
        {
            // If there's a modal window open, close it first
            if (_currentModal != null)
            {
                CloseModal();
                return;
            }            
            
            // Hide current window
            if (_currentWindow != null)
            {
                _currentWindow.Hide();
                _currentWindow = null;
            }

            // Show the previous window from history
            if (_windowHistory.Count != 0)
            {
                _currentWindow = _windowHistory.Pop();
                _currentWindow.Show();
            }
        }

        private void ReplaceWindow(UIWindow newWindow)
        {
            // Hide current window if exists
            if (_currentWindow != null)
            {
                _currentWindow.Hide();
            }

            // Clear history
            _windowHistory.Clear();

            // Set and show new window
            _currentWindow = newWindow;
            newWindow.transform.SetParent(_windowContainer);
            newWindow.Show();
        }

        private void StackWindow(UIWindow newWindow)
        {
            // Just show the new window on top without affecting history or hiding current
            if (_currentWindow != null)
            {
                // Ensure the new window appears on top visually
                newWindow.transform.SetParent(_windowContainer);
                newWindow.transform.SetAsLastSibling();
            }

            // Show new window
            newWindow.Show();
            _currentWindow = newWindow;
        }

        private void PushWindow(UIWindow newWindow)
        {
            // Add current window to history if exists
            if (_currentWindow != null)
            {
                _windowHistory.Push(_currentWindow);
                _currentWindow.Hide();
            }

            // Set and show new window
            _currentWindow = newWindow;
            newWindow.transform.SetParent(_windowContainer);
            newWindow.Show();
        }

        /// <summary>
        /// Check if a window is currently open
        /// </summary>
        public bool IsWindowOpen(UIWindow window)
        {
            return window != null && (window == _currentWindow || window == _currentModal);
        }

        /// <summary>
        /// Check if a window is in the history stack
        /// </summary>
        public bool IsWindowInHistory(UIWindow window)
        {
            return _windowHistory.Contains(window);
        }

        public void CloseIfOpen(UIWindow window)
        {
            if (IsWindowOpen(window))
            {
                Back();
            }
        }
    }
}