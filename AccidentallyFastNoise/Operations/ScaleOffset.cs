using System;
using System.Collections.Generic;
using System.Text;

namespace AccidentallyFastNoise
{
    partial class AFNoise
    {
        public static double ScaleOffset(double value, double scale, double offset)
        {
            return value * scale + offset;
        }
    }
}
