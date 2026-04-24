
namespace Domain.Models
{
    public class SpouseViewDto
    {
        public Guid Id { get; set; }
        public Guid HusbandId { get; set; }
        public Guid WifeId { get; set; }
        public string HusbandName { get; set; }
        public string WifeName { get; set; }
        public int Order { get; set; }
    }
}
