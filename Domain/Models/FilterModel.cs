
namespace Domain.Models;

public class FilterModel
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; } = 10;
    public string SortField { get; set; }  // Maydon nomi ("Name", "Price")
    public bool IsDescending { get; set; } = false; // true bo'lsa kamayish tartibida
                                                    // Mana bu qism JSON-da {"name": "test", "price": "100"} ko'rinishida bo'ladi
    public Dictionary<string, string>? Filters { get; set; }
}
