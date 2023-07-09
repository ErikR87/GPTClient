using GPT.Model;
using GPT.Model.FunctionCalling;
using GPTClient.Helper;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System.Reflection;

namespace GPT;

public abstract class FunctionResolver
{
    public static ServiceCollection Services { get; set; } = new();

    private static Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

    public static void AddAssemby(string assemblyName)
    {
        var assembly = Assembly.Load(assemblyName);
        assemblies.Add(assemblyName, assembly);
    }

    public static IEnumerable<Function> ToGPTFunction()
    {
        var result = new List<Function>();
        var baseType = typeof(FunctionResolver);

        var functionClasses = assemblies.Select(a => a.Value).SelectMany(a => a.GetTypes().Where(c => baseType.IsAssignableFrom(c)));

        foreach (var functionClass in functionClasses)
        {
            var methods = functionClass.GetMethods();

            foreach (var method in methods)
            {
                var description = method.GetCustomAttribute<DescriptionAttribute>()?.Description;

                if (string.IsNullOrWhiteSpace(description))
                    continue;

                var function = new Function
                {
                    Name = method.Name,
                    Description = description,
                };

                var parameters = method.GetParameters();

                foreach (var parameter in parameters)
                {
                    function.Parameters.Properties.Add(parameter.Name, new FunctionParameterProperty
                    {
                        Type = ToJsonType(parameter.ParameterType.ToString()),
                        Description = parameter.GetCustomAttribute<DescriptionAttribute>()?.Description ?? string.Empty,
                        Enum = parameter.ParameterType.IsEnum ? Enum.GetNames(parameter.ParameterType).ToList() : null,
                    });
                }

                result.Add(function);
            }
        }

        return result;
    }

    public static async Task<string> ExecuteFunction(FunctionCall functionCall)
    {
        var baseType = typeof(FunctionResolver);
        var assembly = Assembly.GetEntryAssembly();

        // Suche die Klasse, die die Methode enthält
        var functionClasses = assemblies.Select(a => a.Value).SelectMany(a => a.GetTypes().Where(c => baseType.IsAssignableFrom(c)));

        foreach (var functionClass in functionClasses)
        {
            var method = functionClass.GetMethods().FirstOrDefault(m => m.Name == functionCall.Name);

            if (method == null)
                continue;


            // Erstelle eine Instanz der Klasse
            var classInstance = Activator.CreateInstance(functionClass);

            // Wandle Argumente-String in ein Objekt-Array um
            var args = JsonConvert.DeserializeObject<Dictionary<string, object>>(functionCall.Arguments);

            var parameters = method.GetParameters();
            var argsValues = args.Select((kv, i) => Convert.ChangeType(kv.Value, parameters[i].ParameterType)).ToArray();


            // Führe die Methode aus
            try
            {
                if(method.ReturnType == typeof(Task<string>))
                {
                    var result = await method.InvokeAsync(classInstance, argsValues);
                    return result.ToString();
                }
                
                return method.Invoke(classInstance, argsValues).ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            
        }

        throw new Exception($"Methode mit Namen {functionCall.Name} nicht gefunden.");
    }

    public static string ToJsonType(string systemType)
    {
        switch (systemType)
        {
            case "System.String":
            case "System.Date":
            case "System.DateTime":
                return "string";
            case "System.Int32":
            case "System.Int64":
            case "System.Decimal":
            case "System.Single":
            case "System.Double":
                return "number";
            case "System.Boolean":
                return "boolean";
            case "System.Object":
                return "object";
            case "System.Array":
                return "array";
            // Hier können Sie weitere Typen hinzufügen, die Sie unterstützen möchten.
            default:
                return "unknown";
        }
    }
}

public class DescriptionAttribute : Attribute
{
    public string Description { get; }

    public DescriptionAttribute(string description)
    {
        Description = description;
    }
}
