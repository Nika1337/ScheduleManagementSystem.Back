

namespace Domain.Models;

public class Schedule : BaseModel
{
    public required Guid ScheduleOfWorkerId { get; set; }
    public required Job JobToPerform { get; set; }
    public required DateOnly ScheduledAtDate { get; set; }
    public required PartOfDay ScheduledAtPartOfDay { get; set; }
    public ScheduleChangeRequest? ScheduleChangeRequest { get; set; }
}
