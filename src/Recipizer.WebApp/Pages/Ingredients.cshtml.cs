using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Recipizer.Core;
using Recipizer.Core.Models;

namespace Recipizer.WebApp.Pages;

public class IngredientsModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly Repository _repository;

    public IngredientsModel(ILogger<IndexModel> logger, Repository repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public IEnumerable<IngredientListModel>? Ingredients { get; set; }

    public async Task OnGetAsync()
    {
        Ingredients = await _repository.GetIngredients();
    }

    public async Task<IActionResult> OnPostAddAsync(long id)
    {
        await _repository.CreateInventoryIngredient(id);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(long id)
    {
        await _repository.DeleteInventoryIngredient(id);
        return RedirectToPage();
    }
}
