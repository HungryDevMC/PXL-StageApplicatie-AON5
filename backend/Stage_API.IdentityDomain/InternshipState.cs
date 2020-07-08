namespace Stage_API.Domain
{
    public enum InternshipState
    {
        New,
        InReviewByTeacher,
        ApprovedByTeacher,
        Rejected,
        InReviewByCoordinator,
        ApprovedByAll,
        RejectedByAll
    }
}