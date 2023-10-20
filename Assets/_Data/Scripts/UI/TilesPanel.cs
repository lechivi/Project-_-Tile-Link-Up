using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesPanel : SaiMonoBehaviour
{
    public static TilesPanel instance;
    [SerializeField] private List<UI_Slot> slots = new List<UI_Slot>();

    private Dictionary<TileSO, int> tileCounts = new Dictionary<TileSO, int>();

    protected override void LoadCompoent()
    {
        base.LoadCompoent();
        if (this.slots.Count == 0)
        {
            UI_Slot[] uiSlots = GetComponentsInChildren<UI_Slot>();
            foreach (UI_Slot uiSlot in uiSlots)
            {
                if (!this.slots.Contains(uiSlot))
                    this.slots.Add(uiSlot);
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        TilesPanel.instance = this;
    }

    public void PlaceTile(TileCtrl tile, Transform destTransform)
    {
        if (tile == null)
        {
            Debug.LogWarning("Invalid variables!", gameObject);
            return;
        }

        tile.transform.SetParent(destTransform);
        tile.transform.localPosition = Vector3.zero;
        tile.transform.rotation = Quaternion.identity;

        this.FindMatches(3);
    }

    public bool AddTileToSlot(TileCtrl tile)
    {
        foreach (UI_Slot uiSlot in this.slots)
        {
            if (!uiSlot.IsHaveTile())
            {
                tile.SetStatic(true);
                tile.Col.isTrigger = true;
                uiSlot.Tile = tile;
                this.AddKeyTileSO(tile.TileSO);


                tile.TileMovement.Move(uiSlot.transform, 0.25f);
                return true;
            }
        }

        return false;
    }

    private void AddKeyTileSO(TileSO tileSO)
    {
        if (tileSO == null) return;

        if (this.tileCounts.ContainsKey(tileSO))
        {
            this.tileCounts[tileSO]++;
        }
        else
        {
            this.tileCounts[tileSO] = 1;
        }
    }

    private bool IsWithinBounds(int index)
    {
        return (index >= 0 && index < this.slots.Count);
    }

    private void FindMatches(int minLength = 3)
    {
        List<TileSO> matches = new List<TileSO>();
        foreach (var kvp in this.tileCounts)
        {
            if (kvp.Value >= minLength)
            {
                matches.Add(kvp.Key as TileSO);
            }
        }


        if (matches.Count > 0)
        {
            foreach (var key in matches)
            {
                this.tileCounts.Remove(key);
            }

            foreach (UI_Slot slot in this.slots)
            {
                if (slot.Tile == null || slot.Tile.TileSO == null) continue;

                if (matches.Contains(slot.Tile.TileSO))
                {
                    Destroy(slot.Tile.gameObject);
                }
            }

            //Move other tile to new slot
        }
    }

}
