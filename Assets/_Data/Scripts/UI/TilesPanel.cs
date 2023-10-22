using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;

public class TilesPanel : SaiMonoBehaviour
{
    public static TilesPanel instance;

    [SerializeField] private List<UI_Slot> slots = new List<UI_Slot>();
    [SerializeField] private List<TileCtrl> listTile = new List<TileCtrl>();
    [SerializeField] private int matchLength = 3;

    private Dictionary<TileSO, int> tileCounts = new Dictionary<TileSO, int>();

    public List<TileCtrl> ListTile { get => this.listTile; }
    public int MatchLength { get => this.matchLength; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
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

    public bool IsHaveEmptySlot()
    {
        foreach (UI_Slot slot in this.slots)
        {
            if (!slot.IsHaveTile())
                return true;
        }

        return false;
    }

    public void PlaceTile(TileCtrl tile, Transform destTransform)
    {
        if (tile == null)
        {
            Debug.LogWarning("Invalid variables!", gameObject);
            return;
        }

        tile.transform.localPosition = Vector3.zero;
        tile.transform.rotation = Quaternion.identity;

        List<TileSO> matches = this.Matches();
        if (matches.Count == 0) return;

        StartCoroutine(this.FindMatches(matches));
    }


    public bool AddTile(TileCtrl tile)
    {
        if (!this.IsHaveEmptySlot())
        {
            if (this.Matches().Count == 0)
            {
                if (UIManager.HasInstance)
                {
                    UIManager.Instance.LosePanel.Show(null);
                }

                if (GameManager.HasInstance)
                {
                    GameManager.Instance.PauseGame();
                }
            }
            return false;
        }

        if (this.tileCounts.ContainsKey(tile.TileSO))
        {
            if (this.tileCounts[tile.TileSO] > this.matchLength)
            {
                this.AddTileToFirstTailEmpty(tile);
                return true;
            }

            else
            {
                int lastSame = -1;
                int firstEmpty = -1;

                for (int i = this.slots.Count - 1; i >= 0; i--)
                {
                    if (this.slots[i].IsHaveTile())
                    {
                        if (firstEmpty < 0)
                            firstEmpty = i + 1;

                        if (this.slots[i].Tile.TileSO == tile.TileSO)
                        {
                            lastSame = i;
                            break;
                        }
                    }
                }

                //if (lastSame < 0 || firstEmpty < 0)
                //{
                //    Debug.Log("LastSame:" + lastSame);
                //    Debug.Log("FirstEmp:" + firstEmpty);
                //    Debug.LogError("Something wrong about index TilesPanel", gameObject);
                //    return false;
                //}

                if (lastSame < 0)
                {
                    this.AddTileToFirstEmpty(tile);
                    return true;
                }

                for (int i = firstEmpty - 1; i > lastSame; i--)
                {
                    this.ShiftTileToIndex(i, i + 1, 0.25f);
                }
            }
        }


        this.AddTileToFirstEmpty(tile);

        return true;
    }

    private bool AddTileToFirstEmpty(TileCtrl tile)
    {
        foreach (UI_Slot uiSlot in this.slots)
        {
            if (!uiSlot.IsHaveTile())
            {
                tile.SetState(false);

                uiSlot.Tile = tile;
                tile.transform.SetParent(uiSlot.transform);
                tile.TileMovement.Move(uiSlot.transform, 0.5f);

                this.AddKeyTileSO(tile.TileSO);
                this.listTile.Add(tile);
                //this.RenewDicKey();

                return true;
            }
        }

        return false;
    }

    private bool AddTileToFirstTailEmpty(TileCtrl tile)
    {
        int index = -1;

        for (int i = this.slots.Count - 1; i >= 0; i--)
        {
            if (this.slots[i].IsHaveTile())
            {
                index = i + 1;
                break;
            }

            if (i == 0)
            {
                index = 0;
                break;
            }
        }

        if (index < 0) return false;

        tile.SetState(false);

        this.slots[index].Tile = tile;
        tile.transform.SetParent(this.slots[index].transform);

        tile.TileMovement.Move(this.slots[index].transform, 0.5f);

        this.AddKeyTileSO(tile.TileSO);
        this.listTile.Add(tile);
        //this.RenewDicKey();

        return true;
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

    private void RenewDicKey()
    {
        this.tileCounts = null;
        foreach (TileCtrl tile in this.listTile)
        {
            if (tile.TileSO == null) continue;
            if (this.tileCounts.ContainsKey(tile.TileSO))
            {
                this.tileCounts[tile.TileSO]++;
            }
            else
            {
                this.tileCounts[tile.TileSO] = 1;
            }
        }
    }

    public void RemoveKeyTileSO(TileSO tileSO, int value)
    {
        this.tileCounts[tileSO] -= value;

        if (this.tileCounts[tileSO] <= 0)
            this.tileCounts.Remove(tileSO);
    }

    private void ShiftTileToIndex(int sourceIndex, int destinationIndex, float timeToMove)
    {
        if (sourceIndex < 0 || destinationIndex < 0) return;

        TileCtrl tile = this.slots[sourceIndex].Tile;
        if (tile == null/* || !tile.gameObject.activeSelf*/) return;

        this.slots[destinationIndex].Tile = tile;
        tile.transform.SetParent(this.slots[destinationIndex].transform);

        tile.TileMovement.Move(this.slots[destinationIndex].transform, timeToMove);

        this.slots[sourceIndex].Tile = null;
    }

    public List<TileSO> Matches()
    {
        List<TileSO> matches = new List<TileSO>();
        foreach (var kvp in this.tileCounts)
        {
            if (kvp.Value >= this.matchLength)
            {
                matches.Add(kvp.Key as TileSO);
            }
        }

        return matches;
    }

    private IEnumerator FindMatches(List<TileSO> matches)
    {
        foreach (var key in matches)
        {
            this.RemoveKeyTileSO(key, this.matchLength);
        }

        yield return new WaitForSeconds(0.25f);

        int lastSame = -1;

        for (int i = this.slots.Count - 1; i >= 0; i--)
        {
            if (this.slots[i].Tile == null || this.slots[i].Tile.TileSO == null) continue;
            if (matches.Contains(this.slots[i].Tile.TileSO))
            {
                this.listTile.Remove(this.slots[i].Tile);
                lastSame = i;
                break;
            }
        }

        if (lastSame < 0)
        {
            Debug.LogError("Something wrong about index TilesPanel", gameObject);
            yield break;
        }

        int firstNextIndex = lastSame + 1;
        int firstSame = lastSame - this.matchLength + 1;

        if (firstNextIndex == this.slots.Count || !this.slots[firstNextIndex].IsHaveTile())
        {
            for (int i = firstSame; i <= lastSame; i++)
            {
                this.listTile.Remove(this.slots[i].Tile);
                
                this.slots[i].Tile.gameObject.SetActive(false);
                this.slots[i].Tile.transform.SetParent(GameObject.Find("PoolTile").transform);

                if (GameplayManager.HasInstance)
                {
                    GameplayManager.Instance.PoolTile.TilesGame.Remove(this.slots[i].Tile);
                }

                this.slots[i].Tile = null;
                this.slots[i].CatchAnimation();
            }
        }

        else
        {
            List<int> indexMatches = new List<int>();

            for (int i = firstSame; i <= lastSame; i++)
            {
                this.slots[i].Tile.gameObject.SetActive(false);
                this.slots[i].Tile.transform.SetParent(GameObject.Find("PoolTile").transform);
                this.slots[i].CatchAnimation();

                indexMatches.Add(i);
            }

            yield return new WaitForSeconds(0.25f);

            foreach (int index in indexMatches)
            {
                this.listTile.Remove(this.slots[index].Tile);

                if (GameplayManager.HasInstance)
                {
                    GameplayManager.Instance.PoolTile.TilesGame.Remove(this.slots[index].Tile);
                }

                this.slots[index].Tile = null;
            }

            for (int i = firstNextIndex; i < this.slots.Count; i++)
            {
                if (!this.slots[i].IsHaveTile()) break;

                this.ShiftTileToIndex(i, firstSame, 0.5f);
                firstSame++;
            }
        }

        if (GameplayManager.HasInstance)
        {
            GameplayManager.Instance.Combo();
            GameplayManager.Instance.ScorePoint();
        }

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_MATCH_NOTIFICATION_PROCESSED_1B);
        }
    }

    public void ReorganizePanel(/*int firstEmpty = -1, int firstNextIndex = -1*/)
    {
        int firstEmpty = -1;
        int firstNextIndex = -1;

        for (int i = 0; i < this.slots.Count; i++)
        {
            if (!this.slots[i].IsHaveTile())
            {
                firstEmpty = i;

                if (i < this.slots.Count - 1)
                {
                    for (int j = i + 1; j < this.slots.Count; j++)
                    {
                        if (this.slots[j].IsHaveTile())
                        {
                            firstNextIndex = j;
                            break;
                        }
                    }
                }

                break;
            }
        }

        if (firstNextIndex < 0) return;

        for (int i = firstNextIndex; i < this.slots.Count; i++)
        {
            if (!this.slots[i].IsHaveTile()) break;

            this.ShiftTileToIndex(i, firstEmpty, 0.5f);
            firstEmpty++;
        }
    }
}