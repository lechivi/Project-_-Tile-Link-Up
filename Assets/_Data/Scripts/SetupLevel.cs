using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupLevel : SaiMonoBehaviour
{
    [SerializeField] private Spawner spawner;
    [SerializeField] private LevelSO level;
    [SerializeField] private int matchCount = 3;
    protected override void LoadCompoent()
    {
        base.LoadCompoent();
        if (this.spawner == null)
            this.spawner = GetComponentInChildren<Spawner>();
    }

    private void Start()
    {
        this.Setup();
    }

    private void Setup()
    {
        if (this.level == null) return;

        TileSO[] tileSOs = this.level.GetTiles();
        for (int i = 0; i < tileSOs.Length; i++)
        {
            for (int j = 0; j < this.matchCount; j++)
            {
                Tile tile = this.spawner.Spawn().GetComponent<Tile>();
                tile.SetTile(tileSOs[i]);
            }
        }
    }
}
