using UnityEditor;
using UnityEngine;
using UnityEditor.UIElements;
using UnityEngine.UIElements;


namespace UIElementsExamples
{
    public class SimpleBindingExampleUXML : EditorWindow
    {
        [MenuItem("Window/UIElementsExamples/Simple Binding Example UXML")]
        public static void ShowDefaultWindow()
        {
            var wnd = GetWindow<SimpleBindingExampleUXML>();
            wnd.titleContent = new GUIContent("Simple Binding UXML");
        }

        public void OnEnable()
        {
            var root = this.rootVisualElement;
            var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UIElements_TEST/SimpleBindingExample_1.uxml");
            visualTree.CloneTree(root);
            OnSelectionChange();
        }

        public void OnSelectionChange()
        {
            GameObject selectedObject = Selection.activeObject as GameObject;
            if (selectedObject != null)
            {
                // Create serialization object
                SerializedObject so = new SerializedObject(selectedObject);
                // Bind it to the root of the hierarchy. It will find the right object to bind to...
                rootVisualElement.Bind(so);
            }
            else
            {
                // Unbind the object from the actual visual element
                rootVisualElement.Unbind();

                // Clear the TextField after the binding is removed
                // (this code is not safe if the Q() returns null)
                rootVisualElement.Q<TextField>("GameObjectName").value = "";
            }
        }
    }
}