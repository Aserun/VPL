using System.Windows;

namespace CaptiveAire.VPL.Extensions
{
    public static class PointExtensions
    {
        public static Vector ToVector(this Point point)
        {
            return new Vector(point.X, point.Y);
        }
    }
}