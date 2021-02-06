using System;
using System.Runtime.CompilerServices;

namespace AccidentallyFastNoise
{
    public static class Helper
    {
        public const Int16 AFN_INLINE = 256; //(Int16)MethodImplOptions.AggressiveInlining;

        [MethodImplAttribute(AFN_INLINE)]
        public static double Clamp(double v, double l, double h)
        {
            if (v < l) v = l;
            if (v > h) v = h;

            return v;
        }

        [MethodImplAttribute(AFN_INLINE)]
        public static int FastFloor(double f) { return (f >= 0 ? (int)f : (int)f - 1); }

        [MethodImplAttribute(AFN_INLINE)]
        public static double Lerp(double f, double a, double b)
        {
            return a + f * (b - a);
        }

        [MethodImplAttribute(AFN_INLINE)]
        public static double CubicLerp(double a, double b, double c, double d, double t)
        {
            double p = (d - c) - (a - b);
            return t * t * t * p + t * t * ((a - b) - p) + t * (c - a) + b;
        }

        [MethodImplAttribute(AFN_INLINE)]
        public static double InterpHermiteFunc(double t) { return t * t * (3 - 2 * t); }

        [MethodImplAttribute(AFN_INLINE)]
        public static double InterpQuinticFunc(double t) { return t * t * t * (t * (t * 6 - 15) + 10); }

        [MethodImplAttribute(AFN_INLINE)]
        public static double Quintic_Blend(double f)
        {
            return f * f * f * (f * (f * 6 - 15) + 10);
        }

        [MethodImplAttribute(AFN_INLINE)]
        public static double Bias(double b, double t)
        {
            return Math.Pow(t, Math.Log(b) / Math.Log(0.5));
        }

        public const double CUBIC_3D_BOUNDING = 1 / (double)(1.5f * 1.5f * 1.5f);

        // Hashing
        private const int X_PRIME = 1619;
        private const int Y_PRIME = 31337;
        private const int Z_PRIME = 6971;
        private const int W_PRIME = 1013;

        [MethodImplAttribute(AFN_INLINE)]
        public static double ValCoord2D(int seed, int x, int y)
        {
            int n = seed;
            n ^= X_PRIME * x;
            n ^= Y_PRIME * y;

            return (n * n * n * 60493) / (double)2147483648.0f;
        }

        [MethodImplAttribute(AFN_INLINE)]
        public static double ValCoord3D(int seed, int x, int y, int z)
        {
            int n = seed;
            n ^= X_PRIME * x;
            n ^= Y_PRIME * y;
            n ^= Z_PRIME * z;

            return (n * n * n * 60493) / (double)2147483648.0f;
        }
    }
}
