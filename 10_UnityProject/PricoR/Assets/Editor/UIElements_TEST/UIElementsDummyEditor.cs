using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(UIElementsDummyScript))]
public class UIElementsDummyEditor : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var visualTree = Resources.Load("UIElementsFile/test_inspector_uxml") as VisualTreeAsset;
        var uxmlVE = visualTree.CloneTree();
        uxmlVE.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Resources/UIElementsFile/test_inspector_styles.uss"));
        return uxmlVE;
    }
}