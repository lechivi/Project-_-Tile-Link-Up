using System;
using UnityEditor;
using UnityEngine;

public class LevelSettingEditorWindow : EditorWindowPlus
{
    private LevelSO[] levels;

    private SerializedProperty serializedProperty;

    private string selectedPropertyPach;
    private string selectedProperty;

    private Vector2 scrollPos;

    private readonly float spaceColumn = 5;
    private readonly float wColumn_Indx = 25;
    private readonly float wColumn_Tile = 75;
    private readonly float wColumn_Chance = 70;
    private readonly float wColumn_Preview = 110;
    private readonly float wColumn_Other = 40;

    [MenuItem("Tool/Level Setting")]
    public static void ShowWindow()
    {
        GetWindow<LevelSettingEditorWindow>("Level Setting");
    }

    private void OnGUI()
    {
        this.levels = GetAllInstances<LevelSO>(typeof(LevelSO));

        this.serializedObject = new SerializedObject(this.levels[0]);

        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical("box", GUILayout.MaxWidth(120), GUILayout.ExpandHeight(true));

        this.DrawSliderBar(this.levels);

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("box", GUILayout.ExpandHeight(true));

        if (this.selectedProperty != null)
        {
            for (int i = 0; i < this.levels.Length; i++)
            {
                if (this.levels[i].name == this.selectedProperty)
                {
                    this.serializedObject = new SerializedObject(this.levels[i]);
                    this.serializedProperty = this.serializedObject.GetIterator();
                    this.serializedProperty.NextVisible(true);

                    GUILayout.Space(5);
                    GUILayout.Label("Level " + (i + 1), EditorStyles.boldLabel);

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("TotalTile", GUILayout.Width(70));
                    levels[i].TotalTile = EditorGUILayout.IntField(levels[i].TotalTile, GUILayout.MaxWidth(100));
                    GUILayout.EndHorizontal();
                    
                    GUILayout.Space(10);
                    this.DrawPanelLevel(levels[i]);
                }
            }

        }
        else
        {
            EditorGUILayout.LabelField("Select an item from the list.");
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();

        this.Apply();
    }

    private void DrawSliderBar(LevelSO[] levels)
    {
        GUILayout.Space(5);

        for (int i = 0; i < this.levels.Length; i++)
        {
            this.DrawLevelRow(this.levels[i], i);
        }

        if (!string.IsNullOrEmpty(this.selectedPropertyPach))
        {
            this.selectedProperty = this.selectedPropertyPach;
        }

        GUILayout.Space(10);

        if (GUILayout.Button("New Level", GUILayout.Height(30)))
        {
            this.CreateNewLevelSO();
        }
    }

    private void DrawLevelRow(LevelSO level, int index)
    {
        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(100), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("Level " + (index + 1)))
        {
            this.selectedPropertyPach = level.name;
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical(GUILayout.MaxWidth(15), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("X"))
        {
            this.selectedProperty = null;
            this.selectedPropertyPach = null;
            this.DeleteAssetData(level);
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }


    private void DrawPanelLevel(LevelSO level)
    {
        if (level.PoolTile != null && level.PoolTile.Count > 0)
        {
            this.DrawTitleRow();

            for (int i = 0; i < level.PoolTile.Count; i++)
            {
                this.DrawRow(level, level.PoolTile[i], i);
            }
        }

        GUILayout.Space(10);
        if (GUILayout.Button("New Tile", GUILayout.Height(30)))
        {
            level.PoolTile.Add(null);
        }
    }

    private void DrawTitleRow()
    {
        EditorGUILayout.BeginHorizontal();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Indx), GUILayout.ExpandHeight(false));

        GUILayout.Label("Idx", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Tile), GUILayout.ExpandHeight(false));

        GUILayout.Label("Tile", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Chance), GUILayout.ExpandHeight(false));

        GUILayout.Label("Chance", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Preview), GUILayout.ExpandHeight(false));

        GUILayout.Label("Preview", EditorStyles.boldLabel);

        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();
    }

    private void DrawRow(LevelSO level, TileInLevel tileInLevel, int index)
    {
        EditorGUILayout.BeginHorizontal();


        //INDEX
        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Indx), GUILayout.ExpandHeight(false));

        GUILayout.Label((index + 1).ToString());

        EditorGUILayout.EndVertical();


        //TILE
        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Tile), GUILayout.ExpandHeight(false));

        //tileInLevel.TileSO = (TileSO)EditorGUILayout.ObjectField(GUIContent.none, tileInLevel.TileSO, typeof(TileSO),
        //    allowSceneObjects: false, options: GUILayout.Width(70));
        if (tileInLevel.TileSO != null)
        {
            GUILayout.Label("Name: " + tileInLevel.TileSO.NameSprite, GUILayout.Width(this.wColumn_Tile));
        }

        if (GUILayout.Button("Select", GUILayout.Width(this.wColumn_Tile)))
        {
            SelectionGirdEditorWindow.ShowWindow(level, index);
        }

        EditorGUILayout.EndVertical();


        //CHANCE
        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Chance), GUILayout.ExpandHeight(false));

        tileInLevel.Chance = EditorGUILayout.IntField(tileInLevel.Chance);

        EditorGUILayout.EndVertical();


        //PREVIEW
        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Preview), GUILayout.ExpandHeight(false));

        if (tileInLevel.TileSO != null)
            this.DrawSpritePreview(tileInLevel.TileSO);
        else
            GUILayout.Space(this.wColumn_Preview);
        EditorGUILayout.EndVertical();


        //UP - DOWN TILE
        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Other), GUILayout.ExpandHeight(false));

        if (index != 0)
        {
            if (GUILayout.Button("Up", GUILayout.Width(50), GUILayout.Height(index != level.PoolTile.Count - 1 ? 20 : 40)))
            {
                level.MoveTileUp(index);
            }
        }
        if (index != level.PoolTile.Count - 1)
        {
            if (GUILayout.Button("Down", GUILayout.Width(50), GUILayout.Height(index != 0 ? 20 : 40)))
            {
                level.MoveTileDown(index);
            }
        }

        EditorGUILayout.EndVertical();


        //DELETE TILE
        GUILayout.Space(this.spaceColumn);
        EditorGUILayout.BeginVertical(GUILayout.Width(this.wColumn_Other), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("X", GUILayout.Width(30)))
        {
            level.RemoveTileAt(index);
        }

        EditorGUILayout.EndVertical();


        EditorGUILayout.EndHorizontal();
    }


    private void DrawSpritePreview(TileSO tile)
    {
        if (tile == null || tile.Sprite == null) return;

        Texture2D texture = AssetPreview.GetAssetPreview(tile.Sprite);
        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }

    private void CreateNewLevelSO()
    {
        string nameLevel = "Level_" + DateTime.Now.ToString("yyyyMMddHHmmss");
        string path = "Assets/_Data/Scripts/ScriptableObject/LevelSO/" + nameLevel + ".asset";

        this.CreateNewAsset(new LevelSO(), path);
    }


}
