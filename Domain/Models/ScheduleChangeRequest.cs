

namespace Domain.Models;

public class ScheduleChangeRequest : BaseModel
{
    public required Schedule ScheduleToChange { get; set; }
    public required DateOnly NewDateUtc { get; set; }
    public required PartOfDay NewPartOfDay { get; set; }
    public required DateTime RequestDateTime { get; set; }
}
