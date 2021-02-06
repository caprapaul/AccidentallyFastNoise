namespace AccidentallyFastNoise
{
    public struct Double3
    {
        public double X;
        public double Y;
        public double Z;

        public Double3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Double3(double x, double y) : this(x, y, 0) { }

        public static explicit operator Double2(Double3 d3) => new Double2(d3.X, d3.Y);

        public static Double3 operator +(Double3 d1, Double3 d2)
        {
            return new Double3(d1.X + d2.X, d1.Y + d2.Y, d1.Z + d2.Z);
        }

        public static Double3 operator *(Double3 d3, double d)
        {
            return new Double3(d3.X * d, d3.Y * d, d3.Z * d);
        }

        public static Double3 operator *(double d, Double3 d3)
        {
            return new Double3(d3.X * d, d3.Y * d, d3.Z * d);
        }
    }
}
