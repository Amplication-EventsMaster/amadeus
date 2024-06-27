namespace TwitterClone.APIs.Dtos;

public class FollowerWhereInput
{
    public DateTime? CreatedAt { get; set; }

    public FollowerIdDto? Follower { get; set; }

    public List<FollowerIdDto>? Followers { get; set; }

    public string? Following { get; set; }

    public string? Id { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
