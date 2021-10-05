using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Point
{
    public int x;
    public int y;

    public Point(int nx, int ny)
    {
        x = nx;
        y = ny;
    }

    public Vector2 ToVector()
    {
        return new Vector2(x, y);
    }

    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }

    public static Point FromVector(Vector2 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    public static Point FromVector(Vector3 v)
    {
        return new Point((int)v.x, (int)v.y);
    }

    public void Mult(int m)
    {
        x *= m;
        y *= m;
    }

    public void Add(Point o)
    {
        x += o.x;
        y += o.y;
    }

    public static Point Mult(Point p, int m)
    {
        return new Point(p.x * m, p.y * m);
    }

    public static Point Add(Point p, Point o)
    {
        return new Point(p.x + o.x, p.y + o.y);
    }

    public static Point Clone(Point p)
    {
        return new Point(p.x, p.y);
    }

    public static Point Zero => new Point(0, 0);

    public static Point One => new Point(1, 1);

    public static Point Up => new Point(0, 1);

    public static Point Down => new Point(0, -1);

    public static Point Left => new Point(-1, 0);

    public static Point Right => new Point(1, 0);
}
