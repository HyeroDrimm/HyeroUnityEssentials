using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

namespace HyeroUnityEssentials
{
    public class ObjectSwapToggle : Toggle
    {
        [SerializeField] private GameObject onObject;
        [SerializeField] private GameObject offObject;

        protected override void Awake()
        {
            base.Awake();
            onValueChanged.AddListener(state =>
            {
                if (onObject != null) onObject.SetActive(state);
                if (offObject != null) offObject.SetActive(!state);
            });
        }
    }
}