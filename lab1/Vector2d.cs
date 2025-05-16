namespace lab1;
using System;

public class Vector2d
{
    public int X { get; set; }
    public int Y { get; set; }

    public Vector2d(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Vector2d(Point2d start, Point2d end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
    }

    public int this[int index]
    {
        get{
            switch (index)
            {
                case 0: return X;
                case 1: return Y;
                default: throw new Exception("Индекс должен быть 0 или 1");
            };
        }
        set
        {
            switch (index)
            {
                case 0: X = value; break;
                case 1: Y = value; break;
                default: throw new Exception("Индекс должен быть 0 или 1");
            }
        }
    }

    public double Length(){
        return Math.Sqrt(X * X + Y * Y);
    }


    public bool Equals(Vector2d other)
    {
        if (other is null) return false;
        return X == other.X && Y == other.Y;
    }

    public override string ToString() => $"({X}, {Y})";

    public static Vector2d operator +(Vector2d a, Vector2d b) => new Vector2d(a.X + b.X, a.Y + b.Y);
    public static Vector2d operator -(Vector2d a, Vector2d b) => new Vector2d(a.X - b.X, a.Y - b.Y);
    public static Vector2d operator *(Vector2d v, int scalar) => new Vector2d(v.X * scalar, v.Y * scalar);
    public static Vector2d operator *(int scalar, Vector2d v) => v * scalar;
    public static Vector2d operator /(Vector2d v, int scalar) => scalar == 0 ? throw new DivideByZeroException("Деление на ноль") : new Vector2d(v.X / scalar, v.Y / scalar);

    // с экземпляром
    public int ScalarMult(Vector2d other) => X * other.X + Y * other.Y;

    //статический
    public static int ScalarMult(Vector2d a, Vector2d b) => a.ScalarMult(b);
}