namespace TwitterClone.APIs.Dtos;

public class RetweetDto : RetweetIdDto
{
    public DateTime CreatedAt { get; set; }

    public TweetIdDto? Tweet { get; set; }

    public DateTime UpdatedAt { get; set; }

    public UserIdDto? User { get; set; }
}
