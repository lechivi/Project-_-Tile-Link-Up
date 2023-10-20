using UnityEngine;

public abstract class TileAbstract : SaiMonoBehaviour
{
    [SerializeField] protected TileCtrl tileCtrl;

    protected override void LoadCompoent()
    {
        base.LoadCompoent();
        if (this.tileCtrl == null )
            this.tileCtrl = transform.parent.GetComponent<TileCtrl>();
    }
}
