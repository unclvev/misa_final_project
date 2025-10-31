namespace SalesManagementSystem.Domain.Entity;

public class CustomerType
{
    public ulong Id { get; set; }
    public string TypeName { get; set; } = null!;
    public string? Description { get; set; }
    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();
}


