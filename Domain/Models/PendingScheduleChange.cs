

namespace Domain.Models;

public class PendingScheduleChange : BaseModel
{
    public required Schedule ScheduleToChange { get; set; }
    public required DateOnly NewDate { get; set; }
    public required PartOfDay NewPartOfDay { get; set; }
    public required DateTime RequestDateTime { get; set; }
}
