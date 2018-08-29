using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Geometry;
using Rhino.Geometry;

namespace DataToolsGrasshopper.Utils
{
    internal class AreSimilar
    {
        internal static bool Point3d(Point3d a, Point3d b, double epsilon)
        {
            return Math.Abs(a.X - b.X) < epsilon && Math.Abs(a.Y - b.Y) < epsilon && Math.Abs(a.Z - b.Z) < epsilon;
        }

        internal static bool Vector3d(Vector3d a, Vector3d b, double epsilon)
        {
            return Math.Abs(a.X - b.X) < epsilon && Math.Abs(a.Y - b.Y) < epsilon && Math.Abs(a.Z - b.Z) < epsilon;
        }

        internal static bool Line(Line a, Line b, double epsilon)
        {
            return Point3d(a.From, b.From, epsilon) && Point3d(a.To, b.To, epsilon);
        }

        internal static bool Plane(Rhino.Geometry.Plane a, Rhino.Geometry.Plane b, double epsilon)
        {
            return Point3d(a.Origin, b.Origin, epsilon) && Vector3d(a.XAxis, b.XAxis, epsilon) && Vector3d(a.YAxis, b.YAxis, epsilon);
        }
    }
}
