using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Class for the board's own tile and records their position rather than their position in the array
/// </summary>
public class Tile
{
    private Board board;
    private BoardPosition position;
    public bool isEmpty;

    public Tile(Board board, BoardPosition position, bool empty)
    {
        this.board = board;
        this.position = position;
        this.isEmpty = empty;
    }

    public override string ToString()
    {
        return position.ToString();
    }

    public BoardPosition GetPosition()
    {
        return this.position;
    }

    public void SetPosition(BoardPosition position)
    {
        this.position = position;
    }

}
