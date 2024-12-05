namespace FunProcGen.Distributions;

using RandN;
using RandN.Distributions;
using RandN.Extensions;

public static class Normal
{
    public static IDistribution<double> New()
    {
        // This uses the Box-Muller method.
        var uniformDist = Uniform.New(0d, 1d);
        return
            from u1 in uniformDist
            from u2 in uniformDist
            select
                Math.Sqrt(-2.0 * Math.Log(u1))
                * Math.Cos(2.0 * Math.PI * u2);
    }
}