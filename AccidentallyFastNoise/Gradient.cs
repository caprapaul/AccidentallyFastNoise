using System;
using System.Collections.Generic;
using System.Text;

namespace AccidentallyFastNoise
{
    public partial class AFNoise
    {
        public static double Gradient(Double2 coords, Double2 min, Double2 max)
        {
            double dxM = max.X - min.X;
            double dyM = max.Y - min.Y;

            double len = dxM * dxM + dyM * dyM;

            double dx = coords.X - min.X;
            double dy = coords.Y - min.Y;

            double dp = dx * dxM + dy * dyM;
            dp /= len;

            return dp;
        }
    }
}
