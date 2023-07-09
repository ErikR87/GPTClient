using GPT;

namespace SampleConsoleApp;

/*
You can use so many classes and functions you like for function calling.
It's important to inherit from FunctionResolver and use the [Description()] attribute.
Self-notice: maybe it's better to use interface instead of abstract class.
 */

public class SpecialFunctions : FunctionResolver
{
    [Description("Gibt an, wie das Wetter in einem bestimmten Ort ist.")]
    public string GetWeather([Description("Ort")]string location)
    {
        var temperature = Random.Shared.Next(40);
        return $"in {location} ist es {temperature}°C";
    }
}
