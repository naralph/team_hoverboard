//	Taken from Phidget Spatial-Full cs example folder
//	Adapted to also perform float calculations for Unity - Keyvan Acosta
using System;
using System.Collections.Generic;
using System.Text;

namespace Spatial_full
{
    class SpVector3
    {
        public double X;
        public double Y;
        public double Z;

        public SpVector3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
		
        //Static Vector Operations
        public static SpVector3 CrossProduct(SpVector3 A, SpVector3 B)
        {
            SpVector3 cross = new SpVector3(0, 0, 0);
            cross.X = A.Y * B.Z - A.Z * B.Y;
            cross.Y = A.Z * B.X - A.X * B.Z;
            cross.Z = A.X * B.Y - A.Y * B.X;
            return cross;
        }

        public static double DotProduct(SpVector3 A, SpVector3 B)
        {
            double dot = A.X * B.X + A.Y * B.Y + A.Z * B.Z;
            return dot;
        }

        public static SpVector3 Multiply(double A, SpVector3 B)
        {
            SpVector3 ret = new SpVector3(B.X*A, B.Y*A, B.Z*A);
            return ret;
        }

        public static SpVector3 Subtract(SpVector3 A, SpVector3 B)
        {
            SpVector3 diff = new SpVector3(0, 0, 0);
            diff.X = A.X-B.X;
            diff.Y = A.Y-B.Y;
            diff.Z = A.Z-B.Z;
            return diff;
        }

        public static double Length(SpVector3 A)
        {
            return Math.Sqrt(A.X * A.X + A.Y * A.Y + A.Z * A.Z);
        }

        public static SpVector3 Normalize(SpVector3 A)
        {
            SpVector3 ret = new SpVector3(0,0,0);
            double length = SpVector3.Length(A);
            ret.X = A.X / length;
            ret.Y = A.Y / length;
            ret.Z = A.Z / length;
            return ret;
        }
    }
}
