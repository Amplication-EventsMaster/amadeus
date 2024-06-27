using TwitterClone.APIs.Dtos;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs.Extensions;

public static class LikesExtensions
{
    public static LikeDto ToDto(this Like model)
    {
        return new LikeDto
        {
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            Tweet = new TweetIdDto { Id = model.TweetId },
            UpdatedAt = model.UpdatedAt,
            User = new UserIdDto { Id = model.UserId },
        };
    }

    public static Like ToModel(this LikeUpdateInput updateDto, LikeIdDto idDto)
    {
        var like = new Like { Id = idDto.Id };

        // map required fields
        if (updateDto.CreatedAt != null)
        {
            like.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            like.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return like;
    }
}
