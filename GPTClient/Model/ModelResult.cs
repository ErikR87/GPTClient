namespace GPT.Model;

public class ModelResult
{
    public string _object { get; set; }
    public IEnumerable<Model> data { get; set; }
}

public class Model
{
    public string id { get; set; }
    public string _object { get; set; }
    public int created { get; set; }
    public string owned_by { get; set; }
    public Permission[] permission { get; set; }
    public string root { get; set; }
    public object parent { get; set; }
}

public class Permission
{
    public string id { get; set; }
    public string _object { get; set; }
    public int created { get; set; }
    public bool allow_create_engine { get; set; }
    public bool allow_sampling { get; set; }
    public bool allow_logprobs { get; set; }
    public bool allow_search_indices { get; set; }
    public bool allow_view { get; set; }
    public bool allow_fine_tuning { get; set; }
    public string organization { get; set; }
    public object group { get; set; }
    public bool is_blocking { get; set; }
}
