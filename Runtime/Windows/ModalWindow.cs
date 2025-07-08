using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace HyeroUnityEssentials.WindowSystem
{
    [DefaultExecutionOrder(-10)]
    public class ModalWindow : MonoBehaviour
    {
        private static ModalWindow _instance;
        public static ModalWindow Instance => _instance;

        [SerializeField] private UIWindow uiWindow;
        [SerializeField] private VideoPlayer videoPlayer;
        [SerializeField] private float timeSlowDown;

        [Header("Header")] [SerializeField] private GameObject headerHolder;
        [SerializeField] private TMP_Text headerText;

        [Header("Horizontal Content")] [SerializeField]
        private GameObject horizontalLayoutHolder;

        [SerializeField] private TMP_Text horizontalLayoutText;
        [SerializeField] private Image horizontalLayoutImage;
        [SerializeField] private RawImage horizontalLayoutVideo;

        [Header("Vertical Content")] [SerializeField]
        private GameObject verticalLayoutHolder;

        [SerializeField] private TMP_Text verticalLayoutText;
        [SerializeField] private Image verticalLayoutImage;
        [SerializeField] private RawImage verticalLayoutVideo;

        [Header("Footer")] [SerializeField] private GameObject footerHolder;
        [SerializeField] private Button greenButton;
        [SerializeField] private TMP_Text greenButtonText;
        [SerializeField] private Button redButton;
        [SerializeField] private TMP_Text redButtonText;
        [SerializeField] private Button alternativeButton;
        [SerializeField] private TMP_Text alternativeButtonText;


        private Action onGreenButtonPressed;
        private Action onRedButtonPressed;
        private Action onAlternativeButtonPressed;

        private TimeScaleModifier timeScaleModifier;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            timeScaleModifier = new TimeScaleModifier("modal window", timeSlowDown, 4);

            _instance = this;
            DontDestroyOnLoad(this);

            greenButton.onClick.AddListener(() => onGreenButtonPressed?.Invoke());
            redButton.onClick.AddListener(() => onRedButtonPressed?.Invoke());
            alternativeButton.onClick.AddListener(() => onAlternativeButtonPressed?.Invoke());
        }

        public static void ShowYesNo(string headerText, string contentText, Sprite sprite = null,
            VideoClip video = null, Action onYesAction = null, Action onNoAction = null, string greenButtonText = "Yes",
            string redButtonText = "No", bool zeroTimeScale = true)
        {
            if (Instance == null)
            {
                Debug.LogError("No instance of Modal Window!");
                return;
            }

            Instance.ShowGreenRedImpl(headerText, contentText, sprite, video, onYesAction, onNoAction, greenButtonText,
                redButtonText, zeroTimeScale);
        }

        private void ShowGreenRedImpl(string headerText, string contentText, Sprite sprite = null,
            VideoClip video = null, Action onYesAction = null, Action onNoAction = null, string greenButtonText = "Yes",
            string redButtonText = "No", bool zeroTimeScale = true)
        {
            headerHolder.SetActive(true);
            this.headerText.gameObject.SetActive(true);
            this.headerText.text = headerText;

            horizontalLayoutHolder.SetActive(true);
            verticalLayoutHolder.SetActive(false);

            horizontalLayoutVideo.gameObject.SetActive(video != null);
            videoPlayer.clip = video;
            videoPlayer.Play();

            horizontalLayoutImage.gameObject.SetActive(sprite != null);
            horizontalLayoutImage.sprite = sprite;

            horizontalLayoutText.gameObject.SetActive(true);
            horizontalLayoutText.text = contentText;

            footerHolder.SetActive(true);
            greenButton.gameObject.SetActive(true);
            redButton.gameObject.SetActive(true);
            alternativeButton.gameObject.SetActive(false);

            this.greenButtonText.text = greenButtonText;
            this.redButtonText.text = redButtonText;

            onGreenButtonPressed = WindowManager.Instance.CloseModal;
            if (onGreenButtonPressed != null)
                onGreenButtonPressed += onYesAction;

            onRedButtonPressed = WindowManager.Instance.CloseModal;
            if (onRedButtonPressed != null)
                onRedButtonPressed += onNoAction;

            if (zeroTimeScale)
            {
                TimeController.AddModifier(timeScaleModifier);
                onGreenButtonPressed += () => TimeController.RemoveModifier(timeScaleModifier);
                onRedButtonPressed += () => TimeController.RemoveModifier(timeScaleModifier);
            }

            WindowManager.Instance.ShowModal(uiWindow);
        }

        public static void ShowOk(string headerText, string contentText, Sprite image = null, VideoClip video = null,
            Action onButtonPressed = null, string buttonText = "Ok", bool zeroTimeScale = true)
        {
            if (Instance == null)
            {
                Debug.LogError("No instance of Modal Window!");
                return;
            }

            Instance.ShowOkImpl(headerText, contentText, image, video, onButtonPressed, buttonText, zeroTimeScale);
        }

        private void ShowOkImpl(string headerText, string contentText, Sprite image = null, VideoClip video = null,
            Action onButtonPressed = null, string buttonText = "Ok", bool zeroTimeScale = true)
        {
            headerHolder.SetActive(true);
            this.headerText.gameObject.SetActive(true);
            this.headerText.text = headerText;

            horizontalLayoutHolder.SetActive(true);
            verticalLayoutHolder.SetActive(false);

            horizontalLayoutVideo.gameObject.SetActive(video != null);
            videoPlayer.clip = video;
            videoPlayer.Play();

            horizontalLayoutImage.gameObject.SetActive(image != null);
            horizontalLayoutImage.sprite = image;

            horizontalLayoutText.gameObject.SetActive(true);
            horizontalLayoutText.text = contentText;

            footerHolder.SetActive(true);
            greenButton.gameObject.SetActive(true);
            redButton.gameObject.SetActive(false);
            alternativeButton.gameObject.SetActive(false);

            this.alternativeButtonText.text = buttonText;

            onGreenButtonPressed = WindowManager.Instance.CloseModal;
            if (onButtonPressed != null)
                onGreenButtonPressed += onButtonPressed;

            if (zeroTimeScale)
            {
                TimeController.AddModifier(timeScaleModifier);
                onGreenButtonPressed += () => TimeController.RemoveModifier(timeScaleModifier);
            }

            WindowManager.Instance.ShowModal(uiWindow);
        }
    }
}