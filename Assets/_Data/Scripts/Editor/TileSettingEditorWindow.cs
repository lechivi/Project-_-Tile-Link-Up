using System;
using UnityEditor;
using UnityEngine;

public class TileSettingEditorWindow : EditorWindowPlus
{
    private TileSO[] tiles;
    private Vector2 scrollPos;

    private readonly float spaceColumn = 2;
    private readonly float wColumn_Indx = 30;
    private readonly float wColumn_Name = 50;
    private readonly float wColumn_Sprite = 80;
    private readonly float wColumn_Other = 20;

    [MenuItem("Tool/Tile Setting")]
    public static void ShowWindow()
    {
        GetWindow<TileSettingEditorWindow>("Tile Setting"); ;
    }

    private void OnGUI()
    {
        this.tiles = GetAllInstances<TileSO>(typeof(TileSO));

        this.serializedObject = new SerializedObject(this.tiles[0]);

        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);

        this.DrawTitleRow();

        GUILayout.Space(5);

        for (int i = 0; i < this.tiles.Length; i++)
        {
            this.DrawRow(this.tiles[i], i);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("New Tile", GUILayout.Height(30)))
        {
            this.CreateNewTileSO();
        }

        GUILayout.Space(10);

        EditorGUILayout.EndScrollView();

        this.Apply();
    }

    private void DrawTitleRow()
    {
        EditorGUILayout.BeginHorizontal("box");


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(this.wColumn_Indx), GUILayout.ExpandHeight(false));

        GUILayout.Label("Idx", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(this.wColumn_Name), GUILayout.ExpandHeight(false));

        GUILayout.Label("Name", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(this.wColumn_Sprite), GUILayout.ExpandHeight(false));

        GUILayout.Label("Sprite", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();
    }

    private void DrawRow(TileSO tile, int index)
    {
        EditorGUILayout.BeginHorizontal();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(this.wColumn_Indx), GUILayout.ExpandHeight(false));

        GUILayout.Label(index.ToString());

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(this.wColumn_Name), GUILayout.ExpandHeight(false));

        tile.NameTile = EditorGUILayout.TextField(tile.NameTile);

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(this.wColumn_Sprite), GUILayout.ExpandHeight(false));

        tile.Sprite = (Sprite)EditorGUILayout.ObjectField(GUIContent.none, tile.Sprite, typeof(Sprite),
            allowSceneObjects: false, options: GUILayout.Width(70));

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Other), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("X"))
        {
            this.DeleteAssetData(tile);
        }

        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();
    }

    private void CreateNewTileSO()
    {
        string nameTile = "Tile_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        string path = "Assets/Resources/SO/TileSO/" + nameTile + ".asset";

        this.CreateNewAsset(new TileSO(), path);
    }
}
