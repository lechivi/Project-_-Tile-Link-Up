using System.Collections.Generic;
using UnityEngine;

public class PoolTile : SaiMonoBehaviour
{
    [SerializeField] private TileCtrl tilePrefab;
    [SerializeField] private List<TileCtrl> poolingTile = new List<TileCtrl>();
    [SerializeField] private List<TileCtrl> tilesGame = new List<TileCtrl>();

    [Header("Bounds")]
    [SerializeField] private float boundUp = 11f;
    [SerializeField] private float boundDown = -8f;
    [SerializeField] private float boundRight = 7.5f;
    [SerializeField] private float boundLeft = -7.5f;

    private float boundTop = 0.75f;
    private float boundBot = 4f;
    private float distanceEachSpawn = 4f;

    private List<Vector3> spawnPositions = new List<Vector3>();

    public List<TileCtrl> PoolingTile { get => this.poolingTile; set => this.poolingTile = value; }
    public List<TileCtrl> TilesGame { get => this.tilesGame; set => this.tilesGame = value; }
    private TileCtrl GetTile()
    {
        foreach(TileCtrl tile in this.poolingTile)
        {
            if (tile.gameObject.activeSelf == false)
                return tile;
        }

        TileCtrl newTile = Instantiate(this.tilePrefab, transform).GetComponent<TileCtrl>();
        newTile.gameObject.SetActive(false);
        this.poolingTile.Add(newTile);
        return newTile;
    }

    public TileCtrl SpawnTile()
    {
        int maxLoop = 100;
        int curLoop = 0;
        Vector3 pos;

        do
        {
            if (curLoop > maxLoop)
            {
                pos = Vector3.zero;
                break;
            }
            pos.x = Random.Range(this.boundLeft, this.boundRight);
            pos.z = Random.Range(this.boundDown, this.boundUp);
            pos.y = Random.Range(this.boundBot, this.boundTop);
            curLoop++;
        }
        while (!IsPositionValid(pos));

        TileCtrl tile = this.GetTile();
        tile.SetState(true);
        tile.transform.position = pos;
        tile.transform.rotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        tile.TileMovement.IsMovingTransform = false;
        tile.TileMovement.IsMovingPosition = false;
        tile.gameObject.SetActive(true);

        return tile;
    }

    private bool IsPositionValid(Vector3 pos)
    {
        foreach (Vector3 spawnPos in this.spawnPositions)
        {
            if (Vector3.Distance(pos, spawnPos) < this.distanceEachSpawn)
            {
                return false;
            }
        }
        return true;
    }

    public void SetInactiveTile()
    {
        foreach(TileCtrl tile in this.poolingTile)
        {
            if (tile.gameObject.activeSelf)
                tile.gameObject.SetActive(false);

            if (tile.transform.parent != transform)
                tile.transform.SetParent(transform);
        }
    }
    //public T Spawn<T>() where T : Component
    //{
    //    int maxLoop = 100;
    //    int curLoop = 0;
    //    Vector3 pos;

    //    do
    //    {
    //        if (curLoop > maxLoop)
    //        {
    //            Debug.Log("return null spawn");
    //            return null;
    //        }
    //        pos.x = Random.Range(this.boundLeft, this.boundRight);
    //        pos.z = Random.Range(this.boundDown, this.boundUp);
    //        pos.y = Random.Range(this.boundBot, this.boundTop);
    //        curLoop++;
    //    }
    //    while (!IsPositionValid(pos));

    //    GameObject spawnObj = Instantiate(this.objPrefab, pos,
    //        Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
    //    return spawnObj.GetComponent<T>();
    //}
}
