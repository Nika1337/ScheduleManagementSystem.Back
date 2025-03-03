namespace PendingScheduleChange.WithdrawPendingScheduleChange;

internal sealed class Request
{
    public required Guid Id { get; init; }
    internal sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }
}