namespace TestTask.DTO
{
    public class ContractDto
    {
        public Guid Id { get; set; }
        public ProductionRoomDto ProductionRoom { get; set; }
        public ProductDto Product { get; set; }
        public int ProductQuantity { get; set; }
    }
}
