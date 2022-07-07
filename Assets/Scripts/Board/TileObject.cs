using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour
{

    [SerializeField] private Image image;
    private Tile tile;

    public void SetTileObject(Tile tile)
    {
        this.tile = tile;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
