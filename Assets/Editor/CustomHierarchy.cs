using UnityEditor;
using UnityEngine;

[InitializeOnLoad] // carrega apenas no editor
public class CustomHierarchy
{
    static CustomHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += RenderObjects;
    }

    private static void RenderObjects(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (gameObject == null) return;
        if (gameObject.TryGetComponent<CustomHeaderObject>(out var customHeaderObject))
        {
            EditorGUI.DrawRect(selectionRect, customHeaderObject.backgroundColor);
            EditorGUI.LabelField(selectionRect, gameObject.name.ToUpper(), new GUIStyle()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState() { textColor = customHeaderObject.textColor }
            });
        }
    }
}
