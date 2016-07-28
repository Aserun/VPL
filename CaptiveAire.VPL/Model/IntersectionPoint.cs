using System.Windows;

namespace CaptiveAire.VPL.Model
{
    public struct IntersectionPoint
    {
        public IntersectionPoint(Point intersection, Point tangent)
        {
            Intersection = intersection;
            Tangent = tangent;
        }

        public Point Intersection { get; private set; }

        public Point Tangent { get; private set; }
    }
}