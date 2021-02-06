using System;
using System.Collections.Generic;
using System.Text;

namespace AccidentallyFastNoise
{
    public partial class AFNoise
    {
        public static Double3 ScaleDomain(Double3 coords, Double3 scale)
        {
            return new Double3(coords.X * scale.Y, coords.Y * scale.Y, coords.Z * scale.Z);
        }
    }
}
