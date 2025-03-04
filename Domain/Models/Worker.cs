

namespace Domain.Models;
public class Worker : BaseModel
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required DateTime StartDate { get; set; }
}
