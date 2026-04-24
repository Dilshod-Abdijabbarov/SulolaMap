
namespace Domain.Models
{
    public class AssignParentDto
    {
        public Guid SpouseId { get; set; }
        public Guid PersonId { get; set; }
        public int Order {  get; set; }
    }
}
