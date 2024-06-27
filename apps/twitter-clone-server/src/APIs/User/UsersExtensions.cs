using TwitterClone.APIs.Dtos;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs.Extensions;

public static class UsersExtensions
{
    public static UserDto ToDto(this User model)
    {
        return new UserDto
        {
            CreatedAt = model.CreatedAt,
            Email = model.Email,
            FirstName = model.FirstName,
            Id = model.Id,
            LastName = model.LastName,
            Likes = model.Likes?.Select(x => new LikeIdDto { Id = x.Id }).ToList(),
            Password = model.Password,
            Retweets = model.Retweets?.Select(x => new RetweetIdDto { Id = x.Id }).ToList(),
            Roles = model.Roles,
            Tweets = model.Tweets?.Select(x => new TweetIdDto { Id = x.Id }).ToList(),
            UpdatedAt = model.UpdatedAt,
            Username = model.Username,
        };
    }

    public static User ToModel(this UserUpdateInput updateDto, UserIdDto idDto)
    {
        var user = new User
        {
            Id = idDto.Id,
            Email = updateDto.Email,
            FirstName = updateDto.FirstName,
            LastName = updateDto.LastName
        };

        // map required fields
        if (updateDto.CreatedAt != null)
        {
            user.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.Password != null)
        {
            user.Password = updateDto.Password;
        }
        if (updateDto.Roles != null)
        {
            user.Roles = updateDto.Roles;
        }
        if (updateDto.UpdatedAt != null)
        {
            user.UpdatedAt = updateDto.UpdatedAt.Value;
        }
        if (updateDto.Username != null)
        {
            user.Username = updateDto.Username;
        }

        return user;
    }
}
