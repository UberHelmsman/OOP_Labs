namespace lab1;
using System;

public class Point2d
{
    public static int WIDTH = 800;
    public static int HEIGHT = 800;

    private int _x;
    private int _y;

    public int X
    {
        get { return _x; }
        set
        {
            if (value < 0 || value > WIDTH)
                throw new Exception($"X должен быть от 0 до {WIDTH}");
            _x = value;
        }
    }

    public int Y
    {
        get { return _y; }
        set
        {
            if (value < 0 || value > HEIGHT)
                throw new Exception($"Y должен быть от 0 до {HEIGHT}");
            _y = value;
        }
    }

    public Point2d(int x, int y)
    {
        X = x;
        Y = y;
    }

    public bool Equals(Point2d other)
    {
        if (other is null) return false;
        return X == other.X && Y == other.Y;
    }

    public override string ToString() => $"({X}, {Y})";
}