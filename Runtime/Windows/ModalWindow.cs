using System;
using HyeroUnityEssentials.WindowSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)]
public class ModalWindow : MonoBehaviour
{
    private static ModalWindow _instance;
    public static ModalWindow Instance => _instance;

    [SerializeField] private UIWindow uiWindow;

    [Header("Header")]
    [SerializeField] private GameObject headerHolder;
    [SerializeField] private TMP_Text headerText;

    [Header("Horizontal Content")]
    [SerializeField] private GameObject horizontalLayoutHolder;
    [SerializeField] private TMP_Text horizontalLayoutText;
    [SerializeField] private Image horizontalLayoutImage;
    
    [Header("Vertical Content")]
    [SerializeField] private GameObject verticalLayoutHolder;
    [SerializeField] private TMP_Text verticalLayoutText;
    [SerializeField] private Image verticalLayoutImage;

    [Header("Footer")]
    [SerializeField] private GameObject footerHolder;
    [SerializeField] private Button greenButton;
    [SerializeField] private TMP_Text greenButtonText;
    [SerializeField] private Button redButton;
    [SerializeField] private TMP_Text redButtonText;
    [SerializeField] private Button alternativeButton;
    [SerializeField] private TMP_Text alternativeButtonText;


    private Action onGreenButtonPressed;
    private Action onRedButtonPressed;
    private Action onAlternativeButtonPressed;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this);

        greenButton.onClick.AddListener(()=> onGreenButtonPressed?.Invoke());
        redButton.onClick.AddListener(()=> onRedButtonPressed?.Invoke());
        alternativeButton.onClick.AddListener(()=> onAlternativeButtonPressed?.Invoke());
    }

    public static void ShowYesNo(string headerText, string contentText, Sprite sprite = null, Action onYesAction = null, Action onNoAction = null, string greenButtonText = "Yes", string redButtonText = "No")
    {
        if (Instance == null)
        {
            Debug.LogError("No instance of Modal Window!");
            return;
        }

        Instance.ShowGreenRedImpl(headerText, contentText, sprite, onYesAction, onNoAction, greenButtonText, redButtonText);
    }

    private void ShowGreenRedImpl(string headerText, string contentText, Sprite sprite = null, Action onYesAction = null, Action onNoAction = null, string greenButtonText = "Yes", string redButtonText = "No")
    {
        headerHolder.SetActive(true);
        this.headerText.gameObject.SetActive(true);
        this.headerText.text = headerText;

        horizontalLayoutHolder.SetActive(true);
        verticalLayoutHolder.SetActive(false);

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

        WindowManager.Instance.ShowModal(uiWindow);
    }

    public static void ShowOk(string headerText, string contentText, Sprite image = null, Action onButtonPressed = null, string buttonText = "Ok")
    {
        if (Instance == null)
        {
            Debug.LogError("No instance of Modal Window!");
            return;
        }

        Instance.ShowOkImpl(headerText, contentText, image, onButtonPressed, buttonText);
    }

    private void ShowOkImpl(string headerText, string contentText, Sprite image = null, Action onButtonPressed = null, string buttonText = "Ok")
    {
        headerHolder.SetActive(true);
        this.headerText.gameObject.SetActive(true);
        this.headerText.text = headerText;

        horizontalLayoutHolder.SetActive(true);
        verticalLayoutHolder.SetActive(false);

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


        WindowManager.Instance.ShowModal(uiWindow);
    }
}
