
using Application.DataTransferObjects.Workers;

namespace Profile.GetProfile;

internal sealed class Mapper : ResponseMapper<Response, WorkerProfileResponse>
{
    public override Response FromEntity(WorkerProfileResponse e)
    {
        return new Response
        {
            FirstName = e.FirstName,
            LastName = e.LastName
        };
    }
}