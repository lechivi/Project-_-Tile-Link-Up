using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSO", menuName = "SO/Tile")]
public class TileSO : ScriptableObject
{
    [SerializeField] private string nameTile = "Tile";
    [SerializeField] private Sprite sprite;

    public string NameTile
    {
        get => this.nameTile;
        set
        {
            this.nameTile = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
    public Sprite Sprite
    {
        get => this.sprite;
        set
        {
            this.sprite = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

}
