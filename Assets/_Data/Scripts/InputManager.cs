using UnityEngine;

public class InputManager : MonoBehaviour
{
    public bool isPC = false;
    private TileCtrl currentTile;

    private void Update()
    {
        if (GameManager.HasInstance)
        {
            if (!GameManager.Instance.IsPlaying) return;
        }

        if (this.isPC)
            this.HandlePCInputTile();
        else
            this.HandleAndroidInputTile();
    }

    private void HandlePCInputTile()
    {
        if (!TilesPanel.instance.IsHaveEmptySlot()) return;

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                TileCtrl tile = hit.collider.GetComponent<TileCtrl>();
                if (tile && (this.currentTile != tile))
                {
                    if (this.currentTile)
                        this.currentTile.TileInputHandle.DropDown();

                    this.currentTile = tile;
                    this.currentTile.TileInputHandle.PickUp();
                }

                if (!tile && this.currentTile)
                {
                    this.currentTile.TileInputHandle.DropDown();
                    this.currentTile = null;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (this.currentTile != null)
            {
                this.currentTile.TileInputHandle.Select();
                this.currentTile = null;
            }
        }
    }

    private void HandleAndroidInputTile()
    {
        if (!TilesPanel.instance.IsHaveEmptySlot()) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    TileCtrl tile = hit.collider.GetComponent<TileCtrl>();
                    if (tile && (this.currentTile != tile))
                    {
                        if (this.currentTile)
                            this.currentTile.TileInputHandle.DropDown();

                        this.currentTile = tile;
                        this.currentTile.TileInputHandle.PickUp();
                    }

                    if (!tile && this.currentTile)
                    {
                        this.currentTile.TileInputHandle.DropDown();
                        this.currentTile = null;
                    }
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (this.currentTile != null)
                {
                    this.currentTile.TileInputHandle.Select();
                    this.currentTile = null;
                }
            }
        }
    }
}