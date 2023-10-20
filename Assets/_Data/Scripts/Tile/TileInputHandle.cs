using UnityEngine;

public class TileInputHandle : TileAbstract
{
    private bool isSelected;

    public void PickUp()
    {
        Transform tileTransform = this.tileCtrl.transform;
        Vector3 targetPosition = new Vector3(tileTransform.position.x, 0.75f, tileTransform.position.z);

        this.tileCtrl.SetStatic(true);
        this.tileCtrl.Outline.enabled = true;

        this.tileCtrl.TileMovement.Move(targetPosition, 0.1f);
    }

    public void DropDown()
    {
        if (this.isSelected) return;

        this.tileCtrl.SetStatic(false);
        this.tileCtrl.Outline.enabled = false;
        this.tileCtrl.TileMovement.IsMovingPosition = false;
    }

    public void Select()
    {
        bool canAdd = TilesPanel.instance.AddTileToSlot(this.tileCtrl);
        if (canAdd)
        {
            this.isSelected = true;
            this.tileCtrl.Outline.enabled = false;
        }
    }
}
