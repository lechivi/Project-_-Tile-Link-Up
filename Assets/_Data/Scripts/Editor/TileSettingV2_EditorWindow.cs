using System;
using UnityEditor;
using UnityEngine;

public class TileSettingV2_EditorWindow : EditorWindowPlus
{
    private SerializedProperty serializedProperty;
    private TileSO[] tiles;

    private string selectedPropertyPach;
    private string selectedProperty;

    [MenuItem("Tool/Tile Setting V2")]
    public static void ShowWindow()
    {
        GetWindow<TileSettingV2_EditorWindow>("Tile Setting V2"); ;
    }

    private void OnGUI()
    {
        if (this.tiles == null || this.tiles.Length == 0)
        {
            this.tiles = GetAllInstances<TileSO>(typeof(TileSO));
        }

        this.serializedObject = new SerializedObject(this.tiles[0]);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(120), GUILayout.ExpandHeight(true));

        this.DrawSliderBar(this.tiles);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if (this.selectedProperty != null)
        {
            for (int i = 0; i < this.tiles.Length; i++)
            {
                if (this.tiles[i].name == this.selectedProperty)
                {
                    this.serializedObject = new SerializedObject(this.tiles[i]);
                    this.serializedProperty = this.serializedObject.GetIterator();
                    this.serializedProperty.NextVisible(true);
                    this.DrawProperties(this.serializedProperty);
                    this.DrawSpritePreview(this.tiles[i]);
                }
            }

        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list.");
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        this.Apply();
    }

    private void DrawSliderBar(TileSO[] tiles)
    {
        this.DrawTitleRow();

        for (int i = 0; i < tiles.Length; i++)
        {
            this.DrawTileRow(this.tiles[i], i);
        }

        if (!string.IsNullOrEmpty(this.selectedPropertyPach))
        {
            this.selectedProperty = this.selectedPropertyPach;
        }

        GUILayout.Space(10);

        if (GUILayout.Button("New Tile"))
        {
            this.CreateNewTileSO();
        }
    }

    private void DrawSpritePreview(TileSO tile)
    {
        if (tile.Sprite == null) return;

        Texture2D texture = AssetPreview.GetAssetPreview(tile.Sprite);
        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }

    private void DrawTitleRow()
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(100), GUILayout.ExpandHeight(false));

        GUILayout.Label("Name", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(15), GUILayout.ExpandHeight(false));

        GUILayout.Label("Delete", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }
    private void DrawTileRow(TileSO tile, int index)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(100), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("Tile #" + (index + 1)))
        {
            this.selectedPropertyPach = tile.name;
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(15), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("X"))
        {
            this.selectedProperty = null;
            this.selectedPropertyPach = null;
            this.DeleteAssetData(tile);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    private void CreateNewTileSO()
    {
        string nameTile = "Tile_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        string path = "Assets/_Data/Scripts/ScriptableObject/TileSO/" + nameTile + ".asset";

        this.CreateNewAsset(new TileSO(), path);
    }
}
