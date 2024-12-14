using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEntity : MonoBehaviour
{
    private Tile Tile;

    public void SetTile(Tile tile)
    {
        Tile = tile;
    }

    public Tile GetTile()
    {
        return Tile;
    }
}
