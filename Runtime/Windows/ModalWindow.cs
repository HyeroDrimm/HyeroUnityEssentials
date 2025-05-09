using System;
using HyeroUnityEssentials.WindowSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        greenButton.onClick.AddListener(onGreenButtonPressed.Invoke);
        redButton.onClick.AddListener(onRedButtonPressed.Invoke);
        alternativeButton.onClick.AddListener(onAlternativeButtonPressed.Invoke);
    }

    public static void ShowYesNo(string headerText, string contentText, Action onYesAction, Action onNoAction, string greenButtonText = "Yes", string redButtonText = "No")
    {
        if (Instance == null)
        {
            Debug.LogError("No instance of Modal Window!");
            return;
        }

        Instance.ShowGreenRedImpl(headerText, contentText, onYesAction, onNoAction, greenButtonText, redButtonText);
    }

    private void ShowGreenRedImpl(string headerText, string contentText, Action onYesAction, Action onNoAction, string greenButtonText, string redButtonText)
    {
        headerHolder.SetActive(true);
        this.headerText.gameObject.SetActive(true);
        this.headerText.text = headerText;

        horizontalLayoutHolder.SetActive(true);
        verticalLayoutHolder.SetActive(false);

        horizontalLayoutImage.gameObject.SetActive(false);

        horizontalLayoutText.gameObject.SetActive(true);
        horizontalLayoutText.text = contentText;

        footerHolder.SetActive(true);
        greenButton.gameObject.SetActive(true);
        redButton.gameObject.SetActive(true);
        alternativeButton.gameObject.SetActive(false);

        this.greenButtonText.text = greenButtonText;
        this.redButtonText.text = redButtonText;

        onGreenButtonPressed = onYesAction;
        onGreenButtonPressed += WindowManager.Instance.CloseModal;

        onRedButtonPressed = onNoAction;
        onRedButtonPressed += WindowManager.Instance.CloseModal;

        WindowManager.Instance.ShowModal(uiWindow);
    }

    public static void ShowOk(string headerText, string contentText, Action onButtonPressed, string buttonText = "Ok")
    {
        if (Instance == null)
        {
            Debug.LogError("No instance of Modal Window!");
            return;
        }

        Instance.ShowOkImpl(headerText, contentText, onButtonPressed, buttonText);
    }

    private void ShowOkImpl(string headerText, string contentText, Action onButtonPressed, string buttonText = "Ok")
    {
        headerHolder.SetActive(true);
        this.headerText.gameObject.SetActive(true);
        this.headerText.text = headerText;

        horizontalLayoutHolder.SetActive(true);
        verticalLayoutHolder.SetActive(false);

        horizontalLayoutImage.gameObject.SetActive(false);

        horizontalLayoutText.gameObject.SetActive(true);
        horizontalLayoutText.text = contentText;

        footerHolder.SetActive(true);
        greenButton.gameObject.SetActive(false);
        redButton.gameObject.SetActive(false);
        alternativeButton.gameObject.SetActive(true);

        this.alternativeButtonText.text = buttonText;

        onAlternativeButtonPressed = onButtonPressed;
        onAlternativeButtonPressed += WindowManager.Instance.CloseModal;
    }
}
