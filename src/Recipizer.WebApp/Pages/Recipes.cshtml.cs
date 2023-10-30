using Microsoft.AspNetCore.Mvc.RazorPages;

using Recipizer.Core;
using Recipizer.Core.Models;

namespace Recipizer.WebApp.Pages;

public class RecipesModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Repository _repository;

    public RecipesModel(ILogger<IndexModel> logger, Repository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IEnumerable<RecipeListModel>? Recipes { get; set; }

    public async Task OnGetAsync()
    {
        Recipes = (await _repository.GetRecipesWithIngredients())
            .OrderBy(x => x.MissingIngredients.Count)
            .ThenBy(x => x.InventoryIngredients.Count)
            .ThenBy(x => x.AllIngredients.Count);
        ;
    }
}
