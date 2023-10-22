using UnityEngine;

public abstract class TileAbstract : SaiMonoBehaviour
{
    [SerializeField] protected TileCtrl tileCtrl;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.tileCtrl == null )
            this.tileCtrl = transform.parent.GetComponent<TileCtrl>();
    }
}
