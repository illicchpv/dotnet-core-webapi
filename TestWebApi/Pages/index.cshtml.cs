using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }
    public void OnGet()
    {
        // _logger.LogWarning("OnGet");
        Log.Warning("OnGet");
    }

}