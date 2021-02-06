using System;
using System.Collections.Generic;
using System.Text;

namespace AccidentallyFastNoise
{
    public partial class AFNoise
    {
        public static Double3 TranslateDomain(Double3 coords, Double3 offset)
        {
            return coords + offset;
        }
    }
}
