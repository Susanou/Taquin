using System.Diagnostics.SymbolStore;
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


    public void CreateTiles(Transform tilePrefab, Material[] sections)
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

                Transform tileTransform = GameObject.Instantiate(tilePrefab, GetWorldPosition(pos), Quaternion.identity);

                tileTransform.GetComponent<MeshRenderer>().material = sections[x + width*y];
            }
        }
    }

    public Vector3 GetWorldPosition(BoardPosition pos)
    {
        return new Vector3(pos.x, 0, pos.y) * cellSize; //1 is for the offset from the plane
    }

    public BoardPosition GetBoardPosition(Vector3 worldPosition)
    {
        return new BoardPosition(
            Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.z / cellSize)
        );
    }

    public bool isValidMove(BoardPosition position){
        return  position.x >= 0 && 
                position.y >= 0 && 
                position.x < width && 
                position.y < height && // position selected is on the board
                isOrthogonalNeighbor(position) // and position is a neighbor
                ;
    }

    public bool isOrthogonalNeighbor(BoardPosition position)
    {
        // we don't want any diagonal moves which is equivalent to only moves of Manhattan distance 1
        return emptyPos.ManhattanDistance(position) == 1; 
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
