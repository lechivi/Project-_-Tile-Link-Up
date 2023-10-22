using UnityEngine;

public class UI_Slot : SaiMonoBehaviour
{
    [SerializeField] private TileCtrl tile;
    [SerializeField] private Animator animator;

    public TileCtrl Tile { get => this.tile; set => this.tile = value; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.animator == null)
            this.animator = GetComponentInChildren<Animator>();
    }

    public bool IsHaveTile()
    {
        return (GetComponentInChildren<TileCtrl>() != null || this.tile != null);
    }

    public void RemoveTile()
    {
        if (this.tile == null) return;

        this.tile.transform.SetParent(null);
        this.tile = null;
    }

    public void CatchAnimation()
    {
        this.animator.SetTrigger("Catch");
    }
}
