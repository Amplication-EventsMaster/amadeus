using TwitterClone.APIs.Dtos;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs.Extensions;

public static class FollowersExtensions
{
    public static FollowerDto ToDto(this Follower model)
    {
        return new FollowerDto
        {
            CreatedAt = model.CreatedAt,
            Follower = new FollowerIdDto { Id = model.FollowerId },
            Followers = model.Follower?.Select(x => new FollowerIdDto { Id = x.Id }).ToList(),
            Following = model.Following,
            Id = model.Id,
            UpdatedAt = model.UpdatedAt,
        };
    }

    public static Follower ToModel(this FollowerUpdateInput updateDto, FollowerIdDto idDto)
    {
        var follower = new Follower { Id = idDto.Id, Following = updateDto.Following };

        // map required fields
        if (updateDto.CreatedAt != null)
        {
            follower.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            follower.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return follower;
    }
}
