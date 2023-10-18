using UnityEditor;
using UnityEngine;

public class SelectionGirdEditorWindow : EditorWindowPlus
{
    private static LevelSO staticLevel;
    private static int staticIndex;

    private int xCount = 5;
    private int newWidth = 72;
    private int newHeight = 72;
    private Texture2D[] resizedTextures;

    private int selGridInt = -1;
    private Vector2 scrollPos;
    private TileSO[] tiles;

    public static void ShowWindow(LevelSO level, int index)
    {
        staticLevel = level;
        staticIndex = index;
        GetWindow<SelectionGirdEditorWindow>("Select Grid");
    }

    void OnGUI()
    {
        this.tiles = GetAllInstances<TileSO>(typeof(TileSO));

        if (this.resizedTextures == null || this.resizedTextures.Length != this.tiles.Length)
        {
            this.resizedTextures = new Texture2D[this.tiles.Length];
            for (int i = 0; i < this.tiles.Length; i++)
            {
                this.resizedTextures[i] = ResizeTexture(AssetPreview.GetAssetPreview(this.tiles[i].Sprite));
            }
        }

        this.scrollPos = EditorGUILayout.BeginScrollView(this.scrollPos);

        GUILayout.BeginVertical("Box");

        this.selGridInt = GUILayout.SelectionGrid(this.selGridInt, this.resizedTextures, this.xCount);

        GUILayout.EndVertical();

        GUILayout.EndScrollView();

        if (this.selGridInt >= 0 && this.selGridInt < this.tiles.Length)
        {
            this.OnClickSelection(this.selGridInt);
        }
    }

    private Texture2D ResizeTexture(Texture2D originalTexture)
    {
        Texture2D resizedTexture = new Texture2D(this.newWidth, this.newHeight);

        for (int y = 0; y < this.newHeight; y++)
        {
            for (int x = 0; x < this.newWidth; x++)
            {
                Color pixel = originalTexture.GetPixelBilinear((float)x / this.newWidth, (float)y / this.newHeight);
                resizedTexture.SetPixel(x, y, pixel);
            }
        }

        resizedTexture.Apply();

        return resizedTexture;
    }

    private void OnClickSelection(int index)
    {
        if (staticLevel == null || this.tiles[index] == null)
        {
            Debug.LogWarning("Variables are null. Please check your data.");
            return;
        }

        staticLevel.ReplaceTileAt(this.tiles[index], staticIndex);
        this.Close();
    }
}
