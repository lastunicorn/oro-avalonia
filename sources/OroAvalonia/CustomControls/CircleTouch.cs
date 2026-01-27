using System;
using Avalonia;

namespace DustInTheWind.ClockAvalonia.ClearClock.CustomControls;

internal record struct CircleTouch
{
    public double X { get; }

    public double Y { get; }

    public CircleTouch(double x1, double y1, double r1, double x2, double y2)
    {
        double D = Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

        X = (r1 * (x2 - x1)) / D + x1;
        Y = (r1 * (y2 - y1)) / D + y1;
    }

    public CircleTouch(Point c1, double r1, Point c2)
        : this(c1.X, c1.Y, r1, c2.X, c2.Y)
    {
    }

    public static implicit operator Point(CircleTouch circleTouch)
    {
        return new(circleTouch.X, circleTouch.Y);
    }
}
