using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objPrefab;
    public int MaxSpawn = 20;

    [Header("Bounds")]
    [SerializeField] private float boundUp = 11f;
    [SerializeField] private float boundDown = -8f;
    [SerializeField] private float boundRight = 7.5f;
    [SerializeField] private float boundLeft = -7.5f;

    public void Spawn(int max)
    {
        if (this.objPrefab == null) return;

        this.spawnPositions = new List<Vector3>();
        this.objPrefab.SetActive(true);

        for (int i = 0; i < this.MaxSpawn; i++)
        {
            this.Spawn();
        }

        this.objPrefab.SetActive(false);
    }

    public GameObject Spawn()
    {
        int maxLoop = 100;
        int curLoop = 0;
        Vector3 pos;

        do
        {
            if (curLoop > maxLoop)
            {
                Debug.Log("return null spawn");
                return null;
            }
            pos.x = Random.Range(boundLeft, boundRight);
            pos.z = Random.Range(boundDown, boundUp);
            pos.y = Random.Range(0.75f, 1.25f);
            curLoop++;
        }
        while (!IsPositionValid(pos));
        //while (!IsPositionValid(pos))
        //{
        //    pos.x = Random.Range(boundLeft, boundRight);
        //    pos.z = Random.Range(boundDown, boundUp);
        //    pos.y = Random.Range(1.5f, 2.5f);
        //}
        GameObject spawnObj = Instantiate(this.objPrefab, pos, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
        return spawnObj;
    }

    private List<Vector3> spawnPositions = new List<Vector3>();
    private bool IsPositionValid(Vector3 pos)
    {
        foreach (Vector3 spawnPos in spawnPositions)
        {
            if (Vector3.Distance(pos, spawnPos) < 2f)
            {
                return false;
            }
        }
        return true;
    }
}
