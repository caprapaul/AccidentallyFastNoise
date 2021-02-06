using System;
using System.Runtime.CompilerServices;

namespace AccidentallyFastNoise
{
    public enum FractalType
    {
        FBM,
        RIDGEDMULTI,
        BILLOW,
        MULTI,
        HYBRIDMULTI
    }

    public enum BasisType
    {
        VALUE,
        CUBIC
    }

    public enum Interpolation
    {
        Linear,
        Hermite,
        Quintic
    }

    public partial class AFNoise
    {
        public static double Fractal(Double3 coords, FractalType fractalType = FractalType.FBM, BasisType basisType = BasisType.CUBIC, int octaves = 6, double lacunarity = 2.0, double gain = 0.5, double frequency = 0.01, int seed = 1337)
        {
            double bounding = 1;

            double v = 0;

            coords *= frequency;

            switch (fractalType)
            {
                case FractalType.FBM:
                {
                    v = GetFBM(coords, basisType, octaves, lacunarity, gain, seed, bounding);
                    break;
                }
                case FractalType.RIDGEDMULTI:
                {
                    v = GetRidgedMulti(coords, basisType, octaves, lacunarity, gain, seed, bounding);
                    break;
                }
                case FractalType.BILLOW:
                {
                    v = GetBillow(coords, basisType, octaves, lacunarity, gain, seed, bounding);
                    break;
                }
                default:
                {
                    v = GetFBM(coords, basisType, octaves, lacunarity, gain, seed, bounding);
                    break;
                }
            }

            return Helper.Clamp(v, -1.0, 1.0);
        }

        [MethodImplAttribute(Helper.AFN_INLINE)]
        private static double CalculateFractalBounding(int octaves, double gain)
        {
            double amp = gain;
            double ampFractal = 1;
            for (int i = 1; i < octaves; i++)
            {
                ampFractal += amp;
                amp *= gain;
            }

            return 1 / ampFractal;
        }

        private static double SingleValue(Double3 coords, int seed, Interpolation interpolation)
        {
            int x0 = Helper.FastFloor(coords.X);
            int y0 = Helper.FastFloor(coords.Y);
            int z0 = Helper.FastFloor(coords.Z);
            int x1 = x0 + 1;
            int y1 = y0 + 1;
            int z1 = z0 + 1;

            double xs, ys, zs;
            switch (interpolation)
            {
                default:
                case Interpolation.Linear:
                    xs = coords.X - x0;
                    ys = coords.Y - y0;
                    zs = coords.Z - z0;
                    break;
                case Interpolation.Hermite:
                    xs = Helper.InterpHermiteFunc(coords.X - x0);
                    ys = Helper.InterpHermiteFunc(coords.Y - y0);
                    zs = Helper.InterpHermiteFunc(coords.Z - z0);
                    break;
                case Interpolation.Quintic:
                    xs = Helper.InterpQuinticFunc(coords.X - x0);
                    ys = Helper.InterpQuinticFunc(coords.Y - y0);
                    zs = Helper.InterpQuinticFunc(coords.Z - z0);
                    break;
            }

            double xf00 = Helper.Lerp(Helper.ValCoord3D(seed, x0, y0, z0), Helper.ValCoord3D(seed, x1, y0, z0), xs);
            double xf10 = Helper.Lerp(Helper.ValCoord3D(seed, x0, y1, z0), Helper.ValCoord3D(seed, x1, y1, z0), xs);
            double xf01 = Helper.Lerp(Helper.ValCoord3D(seed, x0, y0, z1), Helper.ValCoord3D(seed, x1, y0, z1), xs);
            double xf11 = Helper.Lerp(Helper.ValCoord3D(seed, x0, y1, z1), Helper.ValCoord3D(seed, x1, y1, z1), xs);

            double yf0 = Helper.Lerp(xf00, xf10, ys);
            double yf1 = Helper.Lerp(xf01, xf11, ys);

            return Helper.Lerp(yf0, yf1, zs);
        }

        private static double SingleCubic(Double3 coords, int seed)
        {
            int x1 = Helper.FastFloor(coords.X);
            int y1 = Helper.FastFloor(coords.Y);
            int z1 = Helper.FastFloor(coords.Z);

            int x0 = x1 - 1;
            int y0 = y1 - 1;
            int z0 = z1 - 1;
            int x2 = x1 + 1;
            int y2 = y1 + 1;
            int z2 = z1 + 1;
            int x3 = x1 + 2;
            int y3 = y1 + 2;
            int z3 = z1 + 2;

            double xs = coords.X - (double)x1;
            double ys = coords.Y - (double)y1;
            double zs = coords.Z - (double)z1;

            return Helper.CubicLerp(
                Helper.CubicLerp(
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y0, z0), Helper.ValCoord3D(seed, x1, y0, z0), Helper.ValCoord3D(seed, x2, y0, z0), Helper.ValCoord3D(seed, x3, y0, z0), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y1, z0), Helper.ValCoord3D(seed, x1, y1, z0), Helper.ValCoord3D(seed, x2, y1, z0), Helper.ValCoord3D(seed, x3, y1, z0), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y2, z0), Helper.ValCoord3D(seed, x1, y2, z0), Helper.ValCoord3D(seed, x2, y2, z0), Helper.ValCoord3D(seed, x3, y2, z0), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y3, z0), Helper.ValCoord3D(seed, x1, y3, z0), Helper.ValCoord3D(seed, x2, y3, z0), Helper.ValCoord3D(seed, x3, y3, z0), xs), 
                    ys),

                Helper.CubicLerp(
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y0, z1), Helper.ValCoord3D(seed, x1, y0, z1), Helper.ValCoord3D(seed, x2, y0, z1), Helper.ValCoord3D(seed, x3, y0, z1), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y1, z1), Helper.ValCoord3D(seed, x1, y1, z1), Helper.ValCoord3D(seed, x2, y1, z1), Helper.ValCoord3D(seed, x3, y1, z1), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y2, z1), Helper.ValCoord3D(seed, x1, y2, z1), Helper.ValCoord3D(seed, x2, y2, z1), Helper.ValCoord3D(seed, x3, y2, z1), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y3, z1), Helper.ValCoord3D(seed, x1, y3, z1), Helper.ValCoord3D(seed, x2, y3, z1), Helper.ValCoord3D(seed, x3, y3, z1), xs),
                    ys),

                Helper.CubicLerp(
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y0, z2), Helper.ValCoord3D(seed, x1, y0, z2), Helper.ValCoord3D(seed, x2, y0, z2), Helper.ValCoord3D(seed, x3, y0, z2), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y1, z2), Helper.ValCoord3D(seed, x1, y1, z2), Helper.ValCoord3D(seed, x2, y1, z2), Helper.ValCoord3D(seed, x3, y1, z2), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y2, z2), Helper.ValCoord3D(seed, x1, y2, z2), Helper.ValCoord3D(seed, x2, y2, z2), Helper.ValCoord3D(seed, x3, y2, z2), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y3, z2), Helper.ValCoord3D(seed, x1, y3, z2), Helper.ValCoord3D(seed, x2, y3, z2), Helper.ValCoord3D(seed, x3, y3, z2), xs),
                    ys),
                Helper.CubicLerp(
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y0, z3), Helper.ValCoord3D(seed, x1, y0, z3), Helper.ValCoord3D(seed, x2, y0, z3), Helper.ValCoord3D(seed, x3, y0, z3), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y1, z3), Helper.ValCoord3D(seed, x1, y1, z3), Helper.ValCoord3D(seed, x2, y1, z3), Helper.ValCoord3D(seed, x3, y1, z3), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y2, z3), Helper.ValCoord3D(seed, x1, y2, z3), Helper.ValCoord3D(seed, x2, y2, z3), Helper.ValCoord3D(seed, x3, y2, z3), xs),
                    Helper.CubicLerp(Helper.ValCoord3D(seed, x0, y3, z3), Helper.ValCoord3D(seed, x1, y3, z3), Helper.ValCoord3D(seed, x2, y3, z3), Helper.ValCoord3D(seed, x3, y3, z3), xs),
                    ys),
                zs) * Helper.CUBIC_3D_BOUNDING;

        }

        [MethodImplAttribute(Helper.AFN_INLINE)]
        private static double GetFBM(Double3 coords, BasisType basisType, int octaves, double lacunarity, double gain, int seed, double bounding)
        {
            double sum = SingleCubic(coords, seed);
            double amp = 1;
            int i = 0;

            while (++i < octaves)
            {
                coords *= lacunarity;

                amp *= gain;

                switch (basisType)
                {
                    case BasisType.VALUE:
                        sum += SingleValue(coords, ++seed, Interpolation.Linear) * amp;
                        break;
                    case BasisType.CUBIC:
                        sum += SingleCubic(coords, ++seed) * amp;
                        break;
                    default:
                        sum += SingleCubic(coords, ++seed) * amp;
                        break;
                }

            }

            return sum * bounding;

        }

        [MethodImplAttribute(Helper.AFN_INLINE)]
        private static double GetRidgedMulti(Double3 coords, BasisType basisType, int octaves, double lacunarity, double gain, int seed, double bounding)
        {
            double sum = 1 - Math.Abs(SingleCubic(coords, seed));
            double amp = 1;
            int i = 0;

            while (++i < octaves)
            {
                coords *= lacunarity;

                amp *= gain;

                switch (basisType)
                {
                    case BasisType.VALUE:
                        sum -= (1 - Math.Abs(SingleValue(coords, ++seed, Interpolation.Linear))) * amp;
                        break;
                    case BasisType.CUBIC:
                        sum -= (1 - Math.Abs(SingleCubic(coords, ++seed))) * amp;
                        break;
                    default:
                        sum -= (1 - Math.Abs(SingleCubic(coords, ++seed))) * amp;
                        break;
                }
            }

            return sum;
        }

        [MethodImplAttribute(Helper.AFN_INLINE)]
        private static double GetBillow(Double3 coords, BasisType basisType, int octaves, double lacunarity, double gain, int seed, double bounding)
        {
            double sum = Math.Abs(SingleCubic(coords, seed)) * 2 - 1;
            double amp = 1;
            int i = 0;

            while (++i < octaves)
            {
                coords *= lacunarity;

                amp *= gain;

                switch (basisType)
                {
                    case BasisType.VALUE:
                        sum += (Math.Abs(SingleValue(coords, ++seed, Interpolation.Linear)) * 2 - 1) * amp;
                        break;
                    case BasisType.CUBIC:
                        sum += (Math.Abs(SingleCubic(coords, ++seed)) * 2 - 1) * amp;
                        break;
                    default:
                        sum += (Math.Abs(SingleCubic(coords, ++seed)) * 2 - 1) * amp;
                        break;
                }
            }

            return sum * bounding;

        }
    }
}
