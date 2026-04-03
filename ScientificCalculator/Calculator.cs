namespace ScientificCalculator;

public static class Calculator
{
    public static double Add(double a, double b) => a + b;
    public static double Subtract(double a, double b) => a - b;
    public static double Multiply(double a, double b) => a * b;

    public static double Divide(double a, double b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException("Nuk lejohet pjestimi me zero.");
        }

        return a / b;
    }

    public static double Power(double a, double b) => Math.Pow(a, b);

    public static double Sqrt(double value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Nuk lejohet rrënja katrore e një numri negativ në numra real.");
        }

        return Math.Sqrt(value);
    }

    public static double SinDeg(double degrees) => Math.Sin(DegreesToRadians(degrees));
    public static double CosDeg(double degrees) => Math.Cos(DegreesToRadians(degrees));
    public static double TanDeg(double degrees) => Math.Tan(DegreesToRadians(degrees));

    public static double Log10(double value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Logaritmi është i definuar vetëm për numra > 0.");
        }

        return Math.Log10(value);
    }

    public static double Ln(double value)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Logaritmi natyror është i definuar vetëm për numra > 0.");
        }

        return Math.Log(value);
    }

    public static long Factorial(int n)
    {
        if (n < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(n), "Faktoriali nuk është i definuar për numra negativ.");
        }

        checked
        {
            long result = 1;
            for (var i = 2; i <= n; i++)
            {
                result *= i;
            }

            return result;
        }
    }

    private static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
}
