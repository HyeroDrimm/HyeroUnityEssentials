using System.Collections;
using System.Collections.Generic;
using HyeroUnityEssentials.WindowSystem;
using UnityEngine;

public class UIWindowUtility : MonoBehaviour
{
    [SerializeField] private UIWindow windowToShow;
    [SerializeField] private WindowTransitionType transitionType = WindowTransitionType.Push;

    public void ShowWindow()
    {
        if (windowToShow == null)
        {
            Debug.LogError("windowToShow not set!");
            return;
        }
        WindowManager.Instance.Show(windowToShow, transitionType);
    }

    public void Back() => WindowManager.Instance.Back();
}
