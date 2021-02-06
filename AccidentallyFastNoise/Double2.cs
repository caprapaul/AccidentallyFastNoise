namespace AccidentallyFastNoise
{
    public struct Double2
    {
        public double X;
        public double Y;

        public Double2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Double3(Double2 d2) => new Double3(d2.X, d2.Y);

        public static Double2 operator *(Double2 d2, double d)
        {
            return new Double2(d2.X * d, d2.Y * d);
        }

        public static Double2 operator *(double d, Double2 d2)
        {
            return new Double2(d2.X * d, d2.Y * d);
        }
    }
}
