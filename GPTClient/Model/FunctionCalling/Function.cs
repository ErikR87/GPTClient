namespace GPT.Model.FunctionCalling;

public struct Function
{
    public Function()
    {
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public FunctionParameters Parameters { get; set; } = new FunctionParameters();
}

public struct FunctionParameters
{
    public FunctionParameters()
    {
    }

    public string Type { get; } = "object";
    public Dictionary<string, FunctionParameterProperty> Properties { get; set; } = new Dictionary<string, FunctionParameterProperty>();
}

public struct FunctionParameterProperty
{
    public string Type { get; set; }
    public string? Description { get; set; }
    public List<string>? Enum { get; set; }
}