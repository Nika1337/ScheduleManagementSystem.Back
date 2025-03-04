﻿

namespace Application.DataTransferObjects.Workers;

public record WorkerProfileUpdateRequest
{
    public required Guid Id { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string Email { get; init; }
}
