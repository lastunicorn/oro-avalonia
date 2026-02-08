using Avalonia;
using Avalonia.Media;

namespace DustInTheWind.OroAvalonia.Presentation.CustomControls;

internal class TopLeftCornerShape
{
    public double CornerRadius { get; set; }

    public Geometry ToGeometry()
    {
        PathFigure pathFigure = GenerateTopLeftPathFigure(CornerRadius);

        PathGeometry geometry = new()
        {
            Figures =
            {
                pathFigure
            }
        };

        return geometry;
    }

    private static PathFigure GenerateTopLeftPathFigure(double cornerRadius = 10)
    {
        // size = 100 x 100
        // corner radius = 10 (default)
        //
        // M 18.65,95.01 - Starts from bottom-left corner
        // A 10,10 0 0 1 0,90 - Draw the arc of the bottom-left corner
        // L 0,10 - Draw line to top-left corner
        // A 10,10 0 0 1 10,0 - Draw the arc of the top-left corner
        // L 90,0 - Draw line to top-right corner
        // A 10,10 0 0 1 95.01,18.65 - Draw arc of the top-right corner
        // A 200,200 0 0 1 10,100 - Draw big arc from top-right corner to bottom-left corner
        // Z - Close the path

        CircleTouch touchPointTopRight = new(100 - cornerRadius, cornerRadius, cornerRadius, 200, 200);
        CircleTouch touchPointBottomLeft = new(cornerRadius, 100 - cornerRadius, cornerRadius, 200, 200);

        return new PathFigure()
        {
            // Starts from bottom-left corner
            StartPoint = touchPointBottomLeft,
            IsClosed = true,
            Segments =
            {
                // Draw the arc of the bottom-left corner
                new ArcSegment
                {
                    Point = new Point(0, 100 - cornerRadius),
                    Size = new Size(cornerRadius, cornerRadius),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Clockwise
                },

                // Draw line to top-left corner
                new LineSegment
                {
                    Point = new Point(0, cornerRadius)
                },

                // Draw the arc of the top-left corner
                new ArcSegment
                {
                    Point = new Point(cornerRadius, 0),
                    Size = new Size(cornerRadius, cornerRadius),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Clockwise
                },

                // Draw line to top-right corner
                new LineSegment
                {
                    Point = new Point(100 - cornerRadius, 0)
                },

                // Draw arc of the top-right corner
                new ArcSegment
                {
                    Point = touchPointTopRight,
                    Size = new Size(cornerRadius, cornerRadius),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.Clockwise
                },

                // Draw big arc from top-right corner to bottom-left corner
                new ArcSegment
                {
                    Point = touchPointBottomLeft,
                    Size = new Size(200, 200),
                    RotationAngle = 0,
                    IsLargeArc = false,
                    SweepDirection = SweepDirection.CounterClockwise
                }
            }
        };
    }

    public static implicit operator Geometry(TopLeftCornerShape cornerShape)
    {
        return cornerShape.ToGeometry();
    }
}