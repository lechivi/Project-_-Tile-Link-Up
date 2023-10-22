using UnityEngine;

public class TileInputHandle : TileAbstract
{
    private bool isSelected;

    public bool IsSelected { get => this.isSelected; set => this.isSelected = value; }

    public void PickUp()
    {
        if (this.isSelected) return;

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_PICKUPTILE_BUTTON03);
        }

        Transform tileTransform = this.tileCtrl.transform;
        Vector3 targetPosition = new Vector3(tileTransform.position.x, 0.75f, tileTransform.position.z);

        this.tileCtrl.SetStatic(true);
        this.tileCtrl.Outline.enabled = true;

        this.tileCtrl.TileMovement.Move(targetPosition, 0.5f);
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
        if (this.isSelected) return;

        bool canAdd = TilesPanel.instance.AddTile(this.tileCtrl);
        if (canAdd)
        {
            this.isSelected = true;
            this.tileCtrl.Outline.enabled = false;
            this.tileCtrl.Col.isTrigger = true;
        }
    }
}
