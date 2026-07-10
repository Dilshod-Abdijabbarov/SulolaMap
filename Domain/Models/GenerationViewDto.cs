
namespace Domain.Models;

public class GenerationViewDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int PersonCount { get; set; }
    public List<PersonDto>? Persons { get; set; }
}
