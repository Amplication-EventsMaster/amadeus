using TwitterClone.APIs.Dtos;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs.Extensions;

public static class RetweetsExtensions
{
    public static RetweetDto ToDto(this Retweet model)
    {
        return new RetweetDto
        {
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Tweet = new TweetIdDto { Id = model.TweetId },
            UpdatedAt = model.UpdatedAt,
            User = new UserIdDto { Id = model.UserId },
        };
    }

    public static Retweet ToModel(this RetweetUpdateInput updateDto, RetweetIdDto idDto)
    {
        var retweet = new Retweet { Id = idDto.Id };

        // map required fields
        if (updateDto.CreatedAt != null)
        {
            retweet.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            retweet.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return retweet;
    }
}
