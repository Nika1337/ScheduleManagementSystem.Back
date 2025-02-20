

namespace Domain.Models;

public class ScheduleChangeRequest : BaseModel
{
    public required Schedule scheduleToChange;
    public required DateOnly NewDateUtc;
    public required PartOfDay NewPartOfDay;
}
