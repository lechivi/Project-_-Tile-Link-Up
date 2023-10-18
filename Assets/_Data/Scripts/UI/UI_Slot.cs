using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Slot : MonoBehaviour
{
    [SerializeField] private Tile tile;

    public Tile Tile { get => this.tile; set => this.tile = value; }

    public bool IsHaveTile()
    {
        return GetComponentInChildren<Tile>() != null;
    }

    //public void 
}
