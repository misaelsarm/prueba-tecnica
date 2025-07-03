using System.Text.Json.Serialization;

public class PokeApiResponse
{
    public int Count { get; set; }
    public string? Next { get; set; }
    public string? Previous { get; set; }
    public List<PokemonListItem> Results { get; set; } = [];
}

public class PokemonListItem
{
    public string Name { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class PokemonDetail
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("weight")]
    public int Weight { get; set; }

    [JsonPropertyName("base_experience")]
    public int BaseExperience { get; set; }

    [JsonPropertyName("sprites")]
    public Sprites Sprites { get; set; }
}

public class PokemonSpeciesDetails
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("varieties")]
    public List<Variety> Varieties { get; set; } = [];
}

public class Variety
{
    [JsonPropertyName("is_default")]
    public bool IsDefault { get; set; }

    [JsonPropertyName("pokemon")]
    public PokemonListItem Pokemon { get; set; } = new PokemonListItem();
}

public class Sprites
{
    [JsonPropertyName("front_default")]
    public string Front_Default { get; set; }
}

public class PokemonViewModel
{
    public List<PokemonListItem> PokemonList { get; set; } = [];
    public List<PokemonListItem> SpeciesList { get; set; } = [];
}
