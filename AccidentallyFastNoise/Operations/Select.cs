namespace AccidentallyFastNoise
{
    public partial class AFNoise
    {
        public static double Select(double value, double low, double high, double threshold, double falloff)
        {
            if (falloff > 0.0)
            {
                if (value < (threshold - falloff))
                {
                    return low;
                }
                else if (value > (threshold + falloff))
                {
                    return high;
                }
                else
                {
                    double lower = threshold - falloff;
                    double upper = threshold + falloff;
                    double blend = Helper.Quintic_Blend((value - lower) / (upper - lower));
                    return Helper.Lerp(blend, low, high);
                }
            }
            else
            {
                if (value < threshold)
                {
                    return low;
                }
                else
                {
                    return high;
                }
            }
        }
    }
}
