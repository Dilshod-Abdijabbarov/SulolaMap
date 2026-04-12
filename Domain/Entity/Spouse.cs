
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entity
{
    [Table("spouses")]
    public class Spouse
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("husband_id")]
        public Guid? HusbandId { get; set; }

        [Column("wife_id")]
        public Guid? WifeId { get; set; }

        [Column("order")]
        public int Order { get; set; }//turshush tartibi, 1-xotin, 2-xotin, 3-xotin va hokazo

        [ForeignKey("HusbandId")]
        public virtual Person Husband { get; set; }

        [ForeignKey("WifeId")]
        public virtual Person Wife { get; set; }

        // Bu nikohdan tug'ilgan bolalar ro'yxati
        public virtual ICollection<Person> Children { get; set; }
    }

}
