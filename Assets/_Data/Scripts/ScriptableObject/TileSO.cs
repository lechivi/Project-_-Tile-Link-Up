using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "TileSO", menuName = "SO/Tile")]
public class TileSO : ScriptableObject
{
    [SerializeField] private string nameSprite = "Tile";
    [SerializeField] private Sprite sprite;

    public string NameSprite
    {
        get => this.nameSprite;
        set
        {
            this.nameSprite = value;
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
