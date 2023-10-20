using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Slot : MonoBehaviour
{
    [SerializeField] private TileCtrl tile;

    public TileCtrl Tile { get => this.tile; set => this.tile = value; }

    public bool IsHaveTile()
    {
        return GetComponentInChildren<TileCtrl>() != null;
    }

    //public void 
}
