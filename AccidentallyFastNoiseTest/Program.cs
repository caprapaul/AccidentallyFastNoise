using AccidentallyFastNoise;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentallyFastNoiseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFractalSpeed();
        }

        public static void TestFractalSpeed()
        {
            Stopwatch timer = new Stopwatch();

            int iterations = 1000000;
            double[] values = new double[iterations];

            timer.Start();

            Parallel.For(0, iterations, (index) => Test(index, ref values));


            //for (int i = 0; i < iterations - 1; i++)
            //{
            //    //ThreadPool.QueueUserWorkItem((s) => Test(i, ref values));
            //    Test(i, ref values);
            //}

            timer.Stop();

            Console.WriteLine("Elapsed after {0:0,0} iterations = {1} ms", iterations, timer.ElapsedMilliseconds);
            Console.ReadLine();
        }

        public static void Test(int i, ref double[] values)
        {
            values[i] = AFNoise.Fractal(new Double3(i, i, i), FractalType.FBM, BasisType.CUBIC, 6, 2.0, 0, 0.2, 1337);
        }
    }
}
