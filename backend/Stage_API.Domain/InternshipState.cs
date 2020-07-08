namespace StageAPI.Domain
{
    public enum InternshipState
    {
        BeingProcessed = 0,
        InReviewByTeacher = 1,
        ApprovedByTeacher = 2,
        Rejected = 3,
        InReviewByCoordinator = 4,
        ApprovedByAll = 5
    }
}