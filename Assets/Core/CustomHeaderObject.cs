using UnityEngine;

public class CustomHeaderObject : MonoBehaviour
{
    public Color textColor = Color.white;
    public Color backgroundColor = Color.red;

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorApplication.RepaintHierarchyWindow();
    }
#endif
}