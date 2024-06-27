namespace TwitterClone.APIs.Dtos;

public class RetweetCreateInput
{
    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public TweetIdDto? Tweet { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserIdDto? User { get; set; }
}
