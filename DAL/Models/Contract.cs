using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TestTask.DAL.Models
{
    public class Contract : BaseDbEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Guid ProductionRoomId { get; set; }
        public ProductionRoom ProductionRoom { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int ProductQuantity { get; set; }
    }
}
