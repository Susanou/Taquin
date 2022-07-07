using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// class used for all the board related checks and conversions
public class Board
{

    private int width;
    private int height;
    private float cellSize;
    private Tile[,] tiles; // array to store the position of each tile
    private BoardPosition emptyPos;

    public Board(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        tiles = new Tile[width, height];


        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                BoardPosition pos = new BoardPosition(x, y);
                if (x == y && x == 1)
                {                    
                    tiles[x, y] = new Tile(this, pos, true);
                    emptyPos = pos;
                }

                tiles[x, y] = new Tile(this, pos, false);
            }
        }

    }


    public void CreateTiles(Transform tilePrefab, Sprite[] sections, Transform parent)
    {
        for(int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                BoardPosition pos = new BoardPosition(x, y);
                if (x == y && x == 1)
                {                    
                    continue;
                }

                Transform tileTransform = GameObject.Instantiate(tilePrefab);
                tileTransform.SetParent(parent);
                tileTransform.localPosition = GetWorldPosition(pos);
                tileTransform.localRotation = Quaternion.identity;

                Image section = tileTransform.GetComponent<Image>();
                section.sprite = sections[x + width*y];
                Debug.Log(x + width*y);
            }
        }
    }

    public Vector3 GetWorldPosition(BoardPosition pos)
    {
        return new Vector3(pos.x, pos.y, 0) * cellSize;
    }

    public BoardPosition GetBoardPosition(Vector3 worldPosition)
    {
        return new BoardPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }


    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }
}
