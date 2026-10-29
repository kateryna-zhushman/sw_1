namespace ReceivingSystem.Dal.Models;
public class Receipt
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int Quantity { get; set; }
    public DateTime ReceivedAt { get; set; }
}
