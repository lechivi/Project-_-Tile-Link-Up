using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class TileInLevel
{
    public TileSO TileSO;
    public int Chance = 1;

    public TileInLevel(TileSO tile, int chance = 1)
    {
        this.TileSO = tile;
        this.Chance = chance;
    }
}

[CreateAssetMenu(fileName = "LevelSO", menuName = "SO/Level")]
public class LevelSO : ScriptableObject
{
    [SerializeField] private string nameLevel = "Level 0";
    [SerializeField] private int totalTile = 5;
    [SerializeField] private int playTime = 120;
    [SerializeField] private List<TileInLevel> poolTile = new List<TileInLevel>();

    public string NameLevel
    {
        get => this.nameLevel;
        set
        {
            this.nameLevel = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
    public int TotalTile
    {
        get => this.totalTile;
        set
        {
            this.totalTile = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    } 
    public int PlayTime
    {
        get => this.playTime;
        set
        {
            this.playTime = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
    public List<TileInLevel> PoolTile
    {
        get => this.poolTile;
        set
        {
            this.poolTile = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }

    public int GetTotalChance()
    {
        int totalChance = 0;
        foreach (var tile in this.poolTile)
        {
            totalChance += tile.Chance;
        }

        return totalChance;
    }

    public void AddNewTileToPool(TileSO newTile)
    {
        this.poolTile.Add(new TileInLevel(newTile));
    }

    public void RemoveTileAt(int index)
    {
        if (index < 0 || index > this.poolTile.Count - 1) return;

        this.poolTile.RemoveAt(index);
    }

    public void ReplaceTileAt(TileSO newTile, int index)
    {
        if (index < 0 || index > this.poolTile.Count - 1) return;

        this.poolTile[index].TileSO = newTile;
    }

    public void MoveTileDown(int index)
    {
        if (index < 0 || index >= this.poolTile.Count - 1) return;

        TileInLevel temp = this.poolTile[index];
        this.poolTile[index] = this.poolTile[index + 1];
        this.poolTile[index + 1] = temp;
    }

    public void MoveTileUp(int index)
    {
        if (index <= 0 || index > this.poolTile.Count - 1) return;

        TileInLevel temp = this.poolTile[index];
        this.poolTile[index] = this.poolTile[index - 1];
        this.poolTile[index - 1] = temp;
    }

    public TileSO[] GetTiles()
    {
        if (this.totalTile <= 0) return null;
        TileSO[] tiles = new TileSO[this.totalTile];

        for (int i = 0; i< this.totalTile; i++)
        {
            tiles[i] = this.GetRandomTileSO();
        }

        return tiles;
    }

    private TileSO GetRandomTileSO()
    {
        int totalChance = 0;
        int[] tileChances = new int[this.poolTile.Count];

        for (int i = 0; i < this.poolTile.Count; i++)
        {
            if (this.poolTile[i].TileSO == null || this.poolTile[i].Chance == 0) continue;

            totalChance += this.poolTile[i].Chance;
            tileChances[i] = totalChance;
        }

        if (totalChance == 0 || tileChances.Length == 0) return null;

        int random = Random.Range(1, totalChance + 1);

        for (int i = 0; i < this.poolTile.Count; i++)
        {
            if (this.poolTile[i].TileSO == null || this.poolTile[i].Chance == 0) continue;

            if (random <= tileChances[i])
            {
                return this.poolTile[i].TileSO;
            }
        }

        return null;
    }
}
