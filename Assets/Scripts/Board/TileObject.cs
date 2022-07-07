using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour
{
    private Tile tile;

    public void SetTileObject(Tile tile)
    {
        this.tile = tile;
    }

    public BoardPosition GetPosition()
    {
        return tile.GetPosition();
    }
}
