using UnityEditor;
using UnityEngine;


public class CustomHierarchyMenu : EditorWindow
{
    [MenuItem("GameObject/Create Custom Header")]

    static void CreateCustomHeader(MenuCommand menuCommand)
    {
        var obj = new GameObject("Header");
        Undo.RegisterCreatedObjectUndo(obj, "Create Custom Header");
        GameObjectUtility.SetParentAndAlign(obj, menuCommand.context as GameObject);
        obj.AddComponent<CustomHeaderObject>();
        Selection.activeObject = obj;
    }
}
