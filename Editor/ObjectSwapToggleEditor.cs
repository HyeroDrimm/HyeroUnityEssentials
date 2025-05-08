using HyeroUnityEssentials;
using UnityEditor.UI;
using UnityEditor;

namespace HyeroUnityEssentials
{
    [CustomEditor(typeof(ObjectSwapToggle), true)]
    [CanEditMultipleObjects]

    public class ObjectSwapToggleEditor : ToggleEditor
    {
        SerializedProperty onObjectProperty;
        SerializedProperty offObjectProperty;

        protected override void OnEnable()
        {
            base.OnEnable();
            onObjectProperty = serializedObject.FindProperty("onObject");
            offObjectProperty = serializedObject.FindProperty("offObject");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.PropertyField(onObjectProperty);
            EditorGUILayout.PropertyField(offObjectProperty);
            EditorGUILayout.Space();

            serializedObject.ApplyModifiedProperties();
        }
    }
}