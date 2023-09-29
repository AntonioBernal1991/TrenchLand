using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a struct to represent a position in a grid
public struct GridPosition : IEquatable<GridPosition>
{
    // The x and z coordinates of the grid position
    public int x;
    public int z;

    // Constructor to initialize a GridPosition object with specified x and z coordinates
    public GridPosition(int x, int z)
    {
        this.x = x;
        this.z = z;
    }

    // Override the Equals method to compare two GridPosition objects for equality
    public override bool Equals(object obj)
    {
        return obj is GridPosition position &&
               x == position.x &&
               z == position.z;
    }

    // Implement the IEquatable interface's Equals method for comparing GridPosition objects
    public bool Equals(GridPosition other)
    {
        return this == other;
    }

    // Override the GetHashCode method to generate a hash code for a GridPosition object
    public override int GetHashCode()
    {
        return System.HashCode.Combine(x, z);
    }

    // Override the ToString method to provide a custom string representation of a GridPosition
    public override string ToString()
    {
        return $"x:{x}; z:{z}";
    }

    // Define the equality operator (==) to compare two GridPosition objects for equality
    public static bool operator ==(GridPosition a, GridPosition b)
    {
        return a.x == b.x && a.z == b.z;
    }

    // Define the inequality operator (!=) to compare two GridPosition objects for inequality
    public static bool operator !=(GridPosition a, GridPosition b)
    {
        return !(a == b);
    }

    // Define the addition operator (+) to perform vector-like addition on GridPosition objects
    public static GridPosition operator +(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x + b.x, a.z + b.z);
    }

    // Define the subtraction operator (-) to perform vector-like subtraction on GridPosition objects
    public static GridPosition operator -(GridPosition a, GridPosition b)
    {
        return new GridPosition(a.x - b.x, a.z - b.z);
    }
}

// Define an interface for custom equality comparisons (not used in this script)
public interface IEquatable<T>
{
}





