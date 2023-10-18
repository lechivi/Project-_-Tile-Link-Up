using UnityEditor;

public class EditorWindowPlus : EditorWindow
{
    protected SerializedObject serializedObject;

    public virtual T[] GetAllInstances<T>(System.Type objectType) where T : UnityEngine.Object
    {
        string[] guids = AssetDatabase.FindAssets("t:" + objectType.Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return a;
    }

    protected virtual void CreateNewAsset(UnityEngine.Object obj, string path)
    {
        AssetDatabase.CreateAsset(obj, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    protected virtual void DeleteAssetData(UnityEngine.Object obj)
    {
        var path = AssetDatabase.GetAssetPath(obj);
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.Refresh();
    }

    protected virtual void DrawProperties(SerializedProperty property)
    {
        while (property.NextVisible(false))
        {
            EditorGUILayout.PropertyField(property, true);
        }
    }

    protected virtual void Apply()
    {
        this.serializedObject.ApplyModifiedProperties();
    }
}
