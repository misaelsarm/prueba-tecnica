using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

public class HomeController : Controller
{
    private readonly HttpClient _httpClient;


    private readonly string _baseUrl = "https://pokeapi.co/api/v2";


    public HomeController(IHttpClientFactory httpClientFactory,
        EmailService emailService)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    [HttpGet("")]
    public async Task<IActionResult> Index(string search, string species, int offset = 0)
    {
        int limit = 20;

        string url = $"{_baseUrl}/pokemon?offset={offset}&limit={limit}";

        PokeApiResponse response = await _httpClient.GetFromJsonAsync<PokeApiResponse>(url);

        PokeApiResponse speciesResponse = await _httpClient.GetFromJsonAsync<PokeApiResponse>($"{_baseUrl}/pokemon-species?limit=100000");

        List<PokemonListItem> pokemonList = response?.Results ?? [];

        List<PokemonListItem> speciesList = speciesResponse?.Results ?? [];

        PokemonViewModel viewModel = new()
        {
            PokemonList = pokemonList,
            SpeciesList = [.. speciesList.OrderBy(p => p.Name)]
        };

        //Si estoy filtrando por texto (Nombre de pokemon)
        if (!string.IsNullOrWhiteSpace(search))
        {
            var fullList = await _httpClient.GetFromJsonAsync<PokeApiResponse>(
                $"{_baseUrl}/pokemon?limit=100000");

            viewModel.PokemonList = [.. fullList.Results
                .Where(p => p.Name.Contains(search, StringComparison.OrdinalIgnoreCase))];
        }

        //Si estoy filtrando por especie
        if (!string.IsNullOrEmpty(species))
        {
            var speciesData = await _httpClient.GetFromJsonAsync<PokemonSpeciesDetails>(
                $"https://pokeapi.co/api/v2/pokemon-species/{species}");

            var varieties = speciesData?.Varieties ?? [];

            viewModel.PokemonList = [.. varieties

                .Select(v => v.Pokemon)];
        }


        if (!string.IsNullOrEmpty(species) || !string.IsNullOrEmpty(search))
        {
            ViewBag.Offset = 0;
            ViewBag.HasNext = false;
            ViewBag.HasPrevious = false;
            ViewBag.Species = species;
            ViewBag.Search = search;
        }
        else
        {
            ViewBag.Offset = offset;
            ViewBag.HasNext = response?.Next != null;
            ViewBag.HasPrevious = response?.Previous != null;
        }
        return View(viewModel);
    }

    [HttpGet("/pokemon-details/{name}")]
    public async Task<IActionResult> Details(string name)
    {
        var pokemon = await _httpClient.GetFromJsonAsync<PokemonDetail>($"{_baseUrl}/pokemon/{name}");
        return View(pokemon);
    }


    [HttpPost("/export")]
    public IActionResult ExportToExcel([FromBody] List<PokemonListItem> pokemonData)
    {
        ExcelPackage.License.SetNonCommercialPersonal("PruebaTecnica");

        using var package = new ExcelPackage();

        var worksheet = package.Workbook.Worksheets.Add("Pokemon");

        worksheet.Cells[1, 1].Value = "Name";
        worksheet.Cells[1, 2].Value = "Url";

        for (int i = 0; i < pokemonData.Count; i++)
        {
            worksheet.Cells[i + 2, 1].Value = pokemonData[i].Name;
            worksheet.Cells[i + 2, 2].Value = pokemonData[i].Url;
        }

        worksheet.Cells.AutoFitColumns();

        var stream = new MemoryStream();
        package.SaveAs(stream);
        stream.Position = 0;

        var fileName = $"pokemon-export-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
        return File(stream,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }
}
