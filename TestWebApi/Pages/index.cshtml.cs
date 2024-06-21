using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly InMemoryDbContext _context;

    public static int createCount = 0;

    public List<Product> products { get; set; } = new List<Product>();
    public List<Buyer> buyers { get; set; } = new List<Buyer>();

    public Buyer? selectedBuyer { get; set; } = null;
    public List<Buyer_Product> selectedPurchases { get; set; } = new List<Buyer_Product>();

    public IndexModel(ILogger<IndexModel> logger, InMemoryDbContext context)
    {
        _logger = logger;
        _context = context;
        createCount++; // IndexModel была создана @Model.createCount  раз
    }
    
    public IActionResult OnPost(string? FirstName, string? LastName){
        Log.Warning($"IndexModel.OnPost ============ FirstName {FirstName} LastName {LastName}");
        return Redirect("/4");
    }

    public void OnGet(int? BuyerId, int? add = null, int? del = null)
    {
        // _logger.LogWarning("OnGet");
        Log.Warning($"IndexModel.OnGet BuyerId:{BuyerId} add:{add} del:{del}");

        ViewData["Date"] = DateTime.Now.ToString("yyy-MM-dd hh:mm:ss");
        ViewData["DateUtc"] = DateTime.UtcNow.ToString("yyy-MM-dd hh:mm:ss");

        products = _context.Products.ToList();
        buyers = _context.Buyers.ToList();

        selectedBuyer = buyers.Find(el => el.Id == BuyerId);
        if (selectedBuyer == null)
        {
            Log.Warning($"OnGet (BuyerId == null || BuyerId <= 0)");
            selectedBuyer = null;
            return;
        }
        Log.Warning($"selectedBuyer {BuyerId}");
        int intBuyerId = BuyerId ?? 0;
        if (add != null && del != null)
        {
            Log.Warning($"OnGet ignore (add != null && del != null)");
        }
        else
        {
            var productId = add != null ? add : del;
            if (productId > 0)
            {
                var cartRec = _context.BuyerCarts.FirstOrDefault(el => el.BuyerId == intBuyerId
                    && el.ProductId == productId);
                if (add != null)
                {
                    if (cartRec == null)
                    {
                        cartRec = new BuyerCart { BuyerId = intBuyerId, ProductId = (int)productId };
                        _context.Add(cartRec);
                    }
                    else
                    {
                        cartRec.ProductCount++;
                    }
                    _context.SaveChanges();
                }
                else
                { // del
                    if (cartRec != null)
                    {
                        var cnt = cartRec.ProductCount - 1;
                        if (cnt > 0)
                        {
                            cartRec.ProductCount = cnt;
                        }
                        else
                        {
                            _context.Remove(cartRec);
                        }
                        _context.SaveChanges();
                    }
                    else
                    {
                        Log.Warning($"OnGet Del (cartRec == null)");
                    }
                }

            }
        }

        // var rs = getRs(intBuyerId);
        var rs = _context.getBuyer_Products(intBuyerId);
        selectedPurchases = rs;
    }

    public object getBuyerProductsInfo(int BuyerId)
    {

        var rs = _context.getBuyer_Products(BuyerId);
        var sum = rs.Sum(el => el.Sum);

        // var rs = getRs(BuyerId);
        // decimal sum = rs.Sum(el => (decimal)((dynamic)el).Sum);

        return new { sum = sum, recs = rs }; //  recs = new List<int>()
    }

    public List<dynamic> getRs(int BuyerId)
    {
        return _context.BuyerCarts.Where(el => el.BuyerId == BuyerId).Join(
            _context.Products,
            el => el.ProductId,
            pr => pr.Id,
            (el, pr) => new
            {
                Id = el.Id,
                ProductId = el.ProductId,
                Name = pr.Name,
                Price = pr.Price,
                Count = el.ProductCount,
                Sum = el.ProductCount * pr.Price,
            }
        ).ToList<object>();
    }



}