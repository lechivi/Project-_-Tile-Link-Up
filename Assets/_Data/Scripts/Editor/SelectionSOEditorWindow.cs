using UnityEditor;
using UnityEngine;

public class SelectionSOEditorWindow : EditorWindow
{
    [MenuItem("Tool/Temp/Select SO", false, 10)]
    public static void ShowWindow()
    {
        GetWindow<SelectionSOEditorWindow>("Select SO");
    }
}
