using UnityEngine;

public class InputManager : MonoBehaviour
{
    private TileCtrl currentTile;

    private void Update()
    {
        this.HandleInput();
    }

    private void HandleInput()
    {
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
}
