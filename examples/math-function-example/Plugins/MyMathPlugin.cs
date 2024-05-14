using System.ComponentModel;
using Microsoft.SemanticKernel;
namespace math_function_example.Plugins;

public sealed class MyMathPlugin
{

    [KernelFunction, Description("Add two integers")]
    public static int Add(
        [Description("The first integer to add")] int number1,
        [Description("The second integer to add")] int number2
    )
    {
        return number1 + number2;
    }

    [KernelFunction, Description("Subtract two integers")]
    public static int Subtract(
        [Description("The first integer to subtract from")] int number1,
        [Description("The second integer to subtract away")] int number2
    )
    {
        return number1 - number2;
    }

    [KernelFunction, Description("Multiply two integers.")]
    public static int Multiply(
        [Description("The first integer to multiply")] int number1,
        [Description("The second integer to multiply")] int number2
    )
    {
        return number1 * number2;
    }

    [KernelFunction, Description("Divide two integers. Make sure the second integer is not 0.")]
    public static int Divide(
        [Description("The first integer to multiply")] int number1,
        [Description("The second integer to multiply")] int number2
    )
    {
        return number1 / number2;
    }
    
}