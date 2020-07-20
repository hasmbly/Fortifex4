namespace Fortifex4.Domain.Enums
{
    public enum ProjectStatus
    {
        Draft = 0,
        SubmittedForApproval = 1,
        Approved = 2,
        Returned = -1,
        Rejected = -2
    }
}