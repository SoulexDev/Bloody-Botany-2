using UnityEngine;

public enum MathFunction { Additive, Multiplicative, Exponential, HyperbolicAdd, HyperbolicMult }

public static class MathFunctions
{
    /// <summary>
    /// Returns processed value from desired math function
    /// </summary>
    /// <param name="b">Base number</param>
    /// <param name="c">Coefficient</param>
    /// <param name="x">X param</param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static float GetFromFunction(float b, float c, float x, MathFunction func)
    {
        return func switch
        {
            MathFunction.Additive => Additive(b, c, x),
            MathFunction.Multiplicative => Multiplicative(b, c, x),
            MathFunction.Exponential => Exponential(b, c, x),
            MathFunction.HyperbolicAdd => HyperbolicAdd(b, c, x),
            MathFunction.HyperbolicMult => HyperbolicMult(b, c, x),
            _ => b
        };
    }
    private static float Additive(float b, float c, float x)
    {
        return b + c * x;
    }
    private static float Multiplicative(float b, float c, float x)
    {
        return b * (1 + c * x);
    }
    private static float Exponential(float b, float c, float x)
    {
        return Mathf.Pow(b, c * x);
    }
    private static float HyperbolicAdd(float b, float c, float x)
    {
        float cx = c * x;
        return b + cx / (cx + 1);
    }
    private static float HyperbolicMult(float b, float c, float x)
    {
        float cx = c * x;
        return b * (cx / (cx + 1));
    }
}