using UnityEngine;
using UnityEditor;
using System.Security.Policy;

[CustomEditor(typeof(TileSO))]
public class TileSOEditor : Editor
{
    private TileSO tileSO;

    private void OnEnable()
    {
        this.tileSO = target as TileSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (this.tileSO.Sprite == null) return;

        //this.tileSO.Sprite = this.AddSpriteField(this.tileSO.Sprite, "Sprite");
        Texture2D texture = AssetPreview.GetAssetPreview(this.tileSO.Sprite);
        GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);

        //EditorGUI.ObjectField(GUILayoutUtility.GetLastRect(), this.tileSO.Sprite, texture);
        //GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        //this.tileSO.Sprite = EditorGUILayout.ObjectField(this.tileSO.Sprite, typeof(Sprite), false, 
        //    GUILayout.Height(EditorGUIUtility.singleLineHeight)) as Sprite;
    }

    private Sprite AddSpriteField(Sprite sprite, string label)
    {
        EditorGUILayout.LabelField(label, GUILayout.Width(50));
        return (Sprite)EditorGUILayout.ObjectField(GUIContent.none, sprite, typeof(Sprite), allowSceneObjects: false, options: GUILayout.Width(55));
    }
}
