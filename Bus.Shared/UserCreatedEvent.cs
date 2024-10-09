namespace Bus.Shared
{
    public record  UserCreatedEvent(Guid userId,string Email,string Phone);
    //
    //public record  UserCreatedEvent
    //{
    //    public Guid UserId { get; init; }
    //    public string Email { get; init; }
    //    public string Phone { get; init; }

    //}
}
