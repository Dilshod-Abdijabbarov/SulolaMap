namespace Domain.Models;

public class PagedResult<T>
{
    public int TotalItems { get; set; }
    public List<T> Items { get; set; }
}
