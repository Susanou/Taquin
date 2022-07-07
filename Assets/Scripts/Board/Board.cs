using System.Diagnostics.SymbolStore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the game board
/// Stores a list of tiles and physical game tiles in order to go from Game coordinates to world coordinates
/// This class also handles all of the game logic
/// </summary>
public class Board
{
    public Transform[,] tileObjects;

    private int width;
    private int height;
    private float cellSize;
    private Tile[,] tiles; // array to store the position of each tile
    private BoardPosition emptyPos;
    private Transform[,] solvedBoard;
    private bool randomizing;

    public Board(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        tiles = new Tile[width, height];


        // Generate the board with an empty spot in the middle
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
                else
                    tiles[x, y] = new Tile(this, pos, false);
            }
        }


        PrintBoard();
    }


    public void CreateTiles(Transform tilePrefab, Material[] sections)
    {

        tileObjects = new Transform[width,height];
        solvedBoard = new Transform[width,height];

        // create the associated physical game tiles
        // and keep a reference of the solved board in order to check when the board has been solved
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
                tileTransform.GetComponent<TileObject>().SetTileObject(tiles[x, y]);

                tileObjects[x,y] = tileTransform;
                solvedBoard[x,y] = tileTransform;
            }
        }
    }

    public void RandomizeBoard(int numberOfRandomMove)
    {
        randomizing = true;
        int numberOfMove = 0;
        while(numberOfMove < numberOfRandomMove)
        {
            // 0 = move up      => y-1
            // 1 = move right   => x-1
            // 2 = move down    => y+1
            // 3 = move left    => x+1

            int move = Random.Range(0, 16) % 4; //choose a random move for the middle spot to ensure that the board is solvable
            
            BoardPosition emptyPosition = GetEmptyTilePosition();
            BoardPosition newPosition;

            switch(move)
            {
                case 0:
                    newPosition = new BoardPosition(emptyPosition.x, emptyPosition.y-1);
                    break;
                case 1:
                    newPosition = new BoardPosition(emptyPosition.x-1, emptyPosition.y);
                    break;
                case 2:
                    newPosition = new BoardPosition(emptyPosition.x, emptyPosition.y+1);
                    break;
                default:
                    newPosition = new BoardPosition(emptyPosition.x+1, emptyPosition.y);
                    break;
            }

            

            if(isValidMove(newPosition))
            {
                Debug.Log(newPosition);
                TileObject selectedTile = GetTileObjectFromPosition(newPosition).GetComponent<TileObject>();
                MoveTiles(selectedTile);
                numberOfMove++;
                //Debug.Log(move);
            }
            else
            {
                continue;
            }

        }

        ResetTilePositions();
        randomizing = false;
    }

    private void PrintBoard()
    {
        Debug.Log("Board");

        for(int y = 0; y < height; y++)
        {
            Debug.Log(tiles[width-1, y].isEmpty + " " + tiles[width-2, y].isEmpty + " " + tiles[width-3, y].isEmpty);
        }
    }


    public void ResetTilePositions()
    {
        //PrintBoard();
        
        //Once the position has been randomized
        // We go through each tile object and set their new positions
        foreach (Transform tile in tileObjects)
        {
            if(tile != null)
                tile.transform.position = GetWorldPosition(tile.GetComponent<TileObject>().GetPosition());
        }

        GameBoard.Instance.StartGame();
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

    public bool isValidMove(TileObject tile, BoardPosition endPosition){

        BoardPosition tilePosition = tile.GetPosition();

        return  IsOrthogonalNeighbor(tilePosition) && endPosition == emptyPos;
    }

    public bool isValidMove(BoardPosition position)
    {
        return  position.x >= 0 && 
                position.y >= 0 && 
                position.x < width && 
                position.y < height && // position must be on the board
                IsOrthogonalNeighbor(position); // and also must be a neighbor of the empty position
    }

    private bool IsOrthogonalNeighbor(BoardPosition position)
    {
        // we don't want any diagonal moves which is equivalent to only moves of Manhattan distance 1
        return emptyPos.ManhattanDistance(position) == 1; 
    }
    
    public void MoveTiles(TileObject selectedTile)
    {
        BoardPosition position = selectedTile.GetPosition();

        //Debug.Log(position == emptyPos);

        //change status of board tiles
        tiles[emptyPos.x, emptyPos.y].isEmpty = false;
        tiles[position.x, position.y].isEmpty = true;

        //change position of physical tiles in the array
        tileObjects[position.x, position.y] = null;
        tileObjects[emptyPos.x, emptyPos.y] = selectedTile.transform;

        //change the tile links
        selectedTile.SetTileObject(tiles[emptyPos.x, emptyPos.y]);
        emptyPos = position;

        //PrintBoard();

        if (!randomizing && IsSolved())
        {
            GameBoard.Instance.EndGame(false);
        }
    }

    public bool IsSolved()
    {

        for (int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {

                if(x == y && y == 1) continue; // skip the tile that is supposed to be empty

                if(tileObjects[x, y] == null) // if the tile is null then it isn't in the correct position
                    return false;

                if(tileObjects[x, y] != solvedBoard[x, y])
                    return false;
            }
        }

        return true;
    }

    public Transform GetTileObjectFromPosition(BoardPosition position)
    {
        return tileObjects[position.x, position.y];
    }

    public BoardPosition GetEmptyTilePosition()
    {
        return this.emptyPos;
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
