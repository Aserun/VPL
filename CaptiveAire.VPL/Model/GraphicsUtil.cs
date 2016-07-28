using System;
using System.Windows;
using System.Windows.Media;

namespace CaptiveAire.VPL.Model
{
    public static class GraphicsUtil
    {
        public static Point FindCenter(Point p1, Point p2)
        {
            //http://stackoverflow.com/questions/973039/find-the-center-of-a-line-in-wpf

            var x = (p1.X + p2.X) / 2;
            var y = (p1.Y + p2.Y) / 2;

            return new Point(x, y);
        }

        /// <summary>
        /// Gets a point that is length away from p1 in a perpindicular direction.
        /// </summary>
        /// <param name="p1">The result with be <paramref name="length"/> away from this point.</param>
        /// <param name="p2">This is used to determine the direction of the source line.</param>
        /// <param name="length"></param>
        /// <remarks>
        /// Bastardized from http://stackoverflow.com/a/133952/232566
        /// </remarks>
        /// <returns></returns>
        public static Point GetPerpindicularPoint(Point p1, Point p2, double length)
        {
            //dx = x1-x2
            var dx = p1.X - p2.X;

            //dy = y1 - y2
            var dy = p1.Y - p2.Y;

            //dist = sqrt(dx * dx + dy * dy)
            var dist = Math.Sqrt((dx * dx) + (dy * dy));

            //dx /= dist
            dx /= dist;

            //dy /= dist
            dy /= dist;

            //x4 = x1 - (N / 2) * dy
            var x4 = p1.X - (length * dy);

            //y4 = y1 + (N / 2) * dx
            var y4 = p1.Y + (length * dx);

            return new Point(x4, y4);
        }


        /// <summary>
        /// Finds the closest point to line segment given a point.
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="limitToSegment"></param>
        /// <remarks>Bastardized from http://csharphelper.com/blog/2014/08/find-the-shortest-distance-between-a-point-and-a-line-segment-in-c/ </remarks>
        /// <returns></returns>
        public static Point FindClosestPointOnSegment(Point pt, Point p1, Point p2, bool limitToSegment = true)
        {
            double dx = p2.X - p1.X;
            double dy = p2.Y - p1.Y;

            if ((dx == 0) && (dy == 0))
            {
                // It's a point not a line segment.
                return p1;
            }

            // Calculate the t that minimizes the distance.
            double t = ((pt.X - p1.X) * dx + (pt.Y - p1.Y) * dy) /
                (dx * dx + dy * dy);

            if (limitToSegment)
            {
                // See if this represents one of the segment's
                // end points or a point in the middle.

                if (t < 0)
                {
                    return new Point(p1.X, p1.Y);
                }

                if (t > 1)
                {
                    return new Point(p2.X, p2.Y);
                }
            }
           
            return new Point(p1.X + t * dx, p1.Y + t * dy);
        }

        public static double GetAngle(Point point1, Point point2)
        {
            double xDiff = point2.X - point1.X;
            double yDiff = point2.Y - point1.Y;

            return Math.Atan2(yDiff, xDiff)*180.0/Math.PI;
        }

        public static IntersectionPoint? GetIntersectionPoint(Geometry intersector, Geometry intersectee, double tolerance = 0.001)
        {
            var flattenedIntersector = intersector.GetFlattenedPathGeometry();

            Point intersect;
            Point tangent;

            flattenedIntersector.GetPointAtFractionLength(1, out intersect, out tangent);

            //first, check the end
            if (!intersectee.FillContains(intersect))
                return null;

            double lowerBound = 0;
            double upperBound = 1;

            while ((upperBound - lowerBound) > tolerance)
            {
                double middle = ((upperBound - lowerBound)/2) + lowerBound;

                //Get the point at the middle of the intersector (line)
                flattenedIntersector.GetPointAtFractionLength(middle, out intersect, out tangent);

                if (intersectee.FillContains(intersect))
                {
                    upperBound = middle;
                }
                else
                {
                    lowerBound = middle;
                }
            }

            return new IntersectionPoint(intersect, tangent); 
        }

        /// <summary>
        /// Constains <paramref name="value"/> to <paramref name="size"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Point Bound(this Point value, Size size)
        {
            if (value.X < 0)
            {
                value.X = 0;
            }

            if (value.Y < 0)
            {
                value.Y = 0;
            }

            if (value.X > size.Width)
            {
                value.X = size.Width;
            }

            if (value.Y > size.Height)
            {
                value.Y = size.Height;
            }

            return value;
        }

        public static TParent FindVisualParent<TParent>(DependencyObject reference) 
            where TParent : DependencyObject
        {
            DependencyObject current = reference;

            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                var typed = current as TParent;

                if (typed != null)
                {
                    return typed;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }

        /// <summary>
        /// Finds a visual parent by its name and type
        /// </summary>
        /// <typeparam name="TParent"></typeparam>
        /// <param name="reference"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TParent FindVisualParent<TParent>(DependencyObject reference, string name)
            where TParent : FrameworkElement
        {
            DependencyObject current = reference;

            current = VisualTreeHelper.GetParent(current);

            while (current != null)
            {
                var typed = current as TParent;

                if (typed != null && string.CompareOrdinal(typed.Name, name) == 0)
                {
                    return typed;
                }

                current = VisualTreeHelper.GetParent(current);
            }

            return null;
        }
    }
}