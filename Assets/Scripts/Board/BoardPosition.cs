using System;

public struct BoardPosition : IEquatable<BoardPosition>
{
    public int x;
    public int y;


    public BoardPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override bool Equals(object obj)
    {
        return obj is BoardPosition position &&
               x == position.x &&
               y == position.y;
    }

    public bool Equals(BoardPosition other)
    {
        return this == other;
    }

    public override string ToString()
    {
        // string interpolation (f-string in pzthon)
        return $"x: {x}; z: {y};"; 
    }

    public override int GetHashCode()
    {
        int hashCode = 1553271884;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        return hashCode;
    }

    public static bool operator ==(BoardPosition a, BoardPosition b) {
        {
            return a.x == b.x && a.y == b.y;
        }
    }

    public static bool operator !=(BoardPosition a, BoardPosition b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static BoardPosition operator +(BoardPosition a, BoardPosition b)
    {
        return new BoardPosition(a.x+b.x, a.y+b.y);
    }

    public static BoardPosition operator -(BoardPosition a, BoardPosition b)
    {
        return new BoardPosition(a.x-b.x, a.y-b.y);
    }
}