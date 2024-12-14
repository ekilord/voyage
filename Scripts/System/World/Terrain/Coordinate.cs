using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class Coordinate
{
    [JsonProperty]
    private int X;
    [JsonProperty]
    private int Y;

    public Coordinate(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coordinate GetCoordinates() { return this; }

    public int GetX() { return X; }

    public int GetY() { return Y; }

    public override bool Equals(object obj)
    {
        if (obj is Coordinate other)
        {
            return this.GetX() == other.GetX() && this.GetY() == other.GetY();
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (GetX() * 397) ^ GetY();
    }
}
