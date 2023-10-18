using System;
using UnityEditor;
using UnityEngine;

public class CreateNewTileWindow : EditorWindow
{
    private SerializedObject serializedObject;
    private SerializedProperty serializedProperty;

    protected TileSO[] tiles;
    public TileSO NewTile;

    private void OnGUI()
    {
        this.serializedObject = new SerializedObject(this.NewTile);
        this.serializedProperty = this.serializedObject.GetIterator();
        this.serializedProperty.NextVisible(true);
        this.DrawProperties(this.serializedProperty);
        this.DrawSpritePreview(this.NewTile);

        if (GUILayout.Button("Save"))
        {
            this.tiles = GetAllInstances<TileSO>();
            string nameTile = "Tile_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            AssetDatabase.CreateAsset(this.NewTile, "Assets/_Data/Scripts/ScriptableObject/TileSO/" + nameTile + ".asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Close();
        }


        this.Apply();
    }

    public static T[] GetAllInstances<T>() where T : TileSO
    {
        string[] guids = AssetDatabase.FindAssets("t:" + typeof(T).Name);
        T[] a = new T[guids.Length];
        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
        }
        return a;
    }

    protected void DrawSpritePreview(TileSO tile)
    {
        if (tile.Sprite == null) return;

        Texture2D texture = AssetPreview.GetAssetPreview(tile.Sprite);
        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }

    protected void DrawProperties(SerializedProperty property)
    {
        while (property.NextVisible(false))
        {
            EditorGUILayout.PropertyField(property, true);
        }
    }

    protected void Apply()
    {
        this.serializedObject.ApplyModifiedProperties();
    }
}
