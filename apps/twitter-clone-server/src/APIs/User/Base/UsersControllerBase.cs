using Microsoft.AspNetCore.Mvc;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;

namespace TwitterClone.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class UsersControllerBase : ControllerBase
{
    protected readonly IUsersService _service;

    public UsersControllerBase(IUsersService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one User
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<UserDto>> CreateUser(UserCreateInput input)
    {
        var user = await _service.CreateUser(input);

        return CreatedAtAction(nameof(User), new { id = user.Id }, user);
    }

    /// <summary>
    /// Delete one User
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteUser([FromRoute()] UserIdDto idDto)
    {
        try
        {
            await _service.DeleteUser(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Users
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<UserDto>>> Users([FromQuery()] UserFindMany filter)
    {
        return Ok(await _service.Users(filter));
    }

    /// <summary>
    /// Get one User
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<UserDto>> User([FromRoute()] UserIdDto idDto)
    {
        try
        {
            return await _service.User(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one User
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateUser(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] UserUpdateInput userUpdateDto
    )
    {
        try
        {
            await _service.UpdateUser(idDto, userUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Likes records to User
    /// </summary>
    [HttpPost("{Id}/likes")]
    public async Task<ActionResult> ConnectLikes(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] LikeIdDto[] likesId
    )
    {
        try
        {
            await _service.ConnectLikes(idDto, likesId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Retweets records to User
    /// </summary>
    [HttpPost("{Id}/retweets")]
    public async Task<ActionResult> ConnectRetweets(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] RetweetIdDto[] retweetsId
    )
    {
        try
        {
            await _service.ConnectRetweets(idDto, retweetsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Tweets records to User
    /// </summary>
    [HttpPost("{Id}/tweets")]
    public async Task<ActionResult> ConnectTweets(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] TweetIdDto[] tweetsId
    )
    {
        try
        {
            await _service.ConnectTweets(idDto, tweetsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Likes records from User
    /// </summary>
    [HttpDelete("{Id}/likes")]
    public async Task<ActionResult> DisconnectLikes(
        [FromRoute()] UserIdDto idDto,
        [FromBody()] LikeIdDto[] likesId
    )
    {
        try
        {
            await _service.DisconnectLikes(idDto, likesId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Retweets records from User
    /// </summary>
    [HttpDelete("{Id}/retweets")]
    public async Task<ActionResult> DisconnectRetweets(
        [FromRoute()] UserIdDto idDto,
        [FromBody()] RetweetIdDto[] retweetsId
    )
    {
        try
        {
            await _service.DisconnectRetweets(idDto, retweetsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Tweets records from User
    /// </summary>
    [HttpDelete("{Id}/tweets")]
    public async Task<ActionResult> DisconnectTweets(
        [FromRoute()] UserIdDto idDto,
        [FromBody()] TweetIdDto[] tweetsId
    )
    {
        try
        {
            await _service.DisconnectTweets(idDto, tweetsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Likes records for User
    /// </summary>
    [HttpGet("{Id}/likes")]
    public async Task<ActionResult<List<LikeDto>>> FindLikes(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] LikeFindMany filter
    )
    {
        try
        {
            return Ok(await _service.FindLikes(idDto, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Find multiple Retweets records for User
    /// </summary>
    [HttpGet("{Id}/retweets")]
    public async Task<ActionResult<List<RetweetDto>>> FindRetweets(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] RetweetFindMany filter
    )
    {
        try
        {
            return Ok(await _service.FindRetweets(idDto, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Find multiple Tweets records for User
    /// </summary>
    [HttpGet("{Id}/tweets")]
    public async Task<ActionResult<List<TweetDto>>> FindTweets(
        [FromRoute()] UserIdDto idDto,
        [FromQuery()] TweetFindMany filter
    )
    {
        try
        {
            return Ok(await _service.FindTweets(idDto, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Meta data about User records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> UsersMeta([FromQuery()] UserFindMany filter)
    {
        return Ok(await _service.UsersMeta(filter));
    }

    /// <summary>
    /// Update multiple Likes records for User
    /// </summary>
    [HttpPatch("{Id}/likes")]
    public async Task<ActionResult> UpdateLikes(
        [FromRoute()] UserIdDto idDto,
        [FromBody()] LikeIdDto[] likesId
    )
    {
        try
        {
            await _service.UpdateLikes(idDto, likesId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Update multiple Retweets records for User
    /// </summary>
    [HttpPatch("{Id}/retweets")]
    public async Task<ActionResult> UpdateRetweets(
        [FromRoute()] UserIdDto idDto,
        [FromBody()] RetweetIdDto[] retweetsId
    )
    {
        try
        {
            await _service.UpdateRetweets(idDto, retweetsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Update multiple Tweets records for User
    /// </summary>
    [HttpPatch("{Id}/tweets")]
    public async Task<ActionResult> UpdateTweets(
        [FromRoute()] UserIdDto idDto,
        [FromBody()] TweetIdDto[] tweetsId
    )
    {
        try
        {
            await _service.UpdateTweets(idDto, tweetsId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
