using Microsoft.AspNetCore.Mvc;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;

namespace TwitterClone.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class TweetsControllerBase : ControllerBase
{
    protected readonly ITweetsService _service;

    public TweetsControllerBase(ITweetsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Tweet
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<TweetDto>> CreateTweet(TweetCreateInput input)
    {
        var tweet = await _service.CreateTweet(input);

        return CreatedAtAction(nameof(Tweet), new { id = tweet.Id }, tweet);
    }

    /// <summary>
    /// Delete one Tweet
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteTweet([FromRoute()] TweetIdDto idDto)
    {
        try
        {
            await _service.DeleteTweet(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Tweets
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<TweetDto>>> Tweets([FromQuery()] TweetFindMany filter)
    {
        return Ok(await _service.Tweets(filter));
    }

    /// <summary>
    /// Get one Tweet
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<TweetDto>> Tweet([FromRoute()] TweetIdDto idDto)
    {
        try
        {
            return await _service.Tweet(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Connect multiple Likes records to Tweet
    /// </summary>
    [HttpPost("{Id}/likes")]
    public async Task<ActionResult> ConnectLikes(
        [FromRoute()] TweetIdDto idDto,
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
    /// Connect multiple Retweets records to Tweet
    /// </summary>
    [HttpPost("{Id}/retweets")]
    public async Task<ActionResult> ConnectRetweets(
        [FromRoute()] TweetIdDto idDto,
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
    /// Disconnect multiple Likes records from Tweet
    /// </summary>
    [HttpDelete("{Id}/likes")]
    public async Task<ActionResult> DisconnectLikes(
        [FromRoute()] TweetIdDto idDto,
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
    /// Disconnect multiple Retweets records from Tweet
    /// </summary>
    [HttpDelete("{Id}/retweets")]
    public async Task<ActionResult> DisconnectRetweets(
        [FromRoute()] TweetIdDto idDto,
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
    /// Find multiple Likes records for Tweet
    /// </summary>
    [HttpGet("{Id}/likes")]
    public async Task<ActionResult<List<LikeDto>>> FindLikes(
        [FromRoute()] TweetIdDto idDto,
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
    /// Find multiple Retweets records for Tweet
    /// </summary>
    [HttpGet("{Id}/retweets")]
    public async Task<ActionResult<List<RetweetDto>>> FindRetweets(
        [FromRoute()] TweetIdDto idDto,
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
    /// Get a User record for Tweet
    /// </summary>
    [HttpGet("{Id}/users")]
    public async Task<ActionResult<List<UserDto>>> GetUser([FromRoute()] TweetIdDto idDto)
    {
        var user = await _service.GetUser(idDto);
        return Ok(user);
    }

    /// <summary>
    /// Meta data about Tweet records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> TweetsMeta([FromQuery()] TweetFindMany filter)
    {
        return Ok(await _service.TweetsMeta(filter));
    }

    /// <summary>
    /// Update multiple Likes records for Tweet
    /// </summary>
    [HttpPatch("{Id}/likes")]
    public async Task<ActionResult> UpdateLikes(
        [FromRoute()] TweetIdDto idDto,
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
    /// Update multiple Retweets records for Tweet
    /// </summary>
    [HttpPatch("{Id}/retweets")]
    public async Task<ActionResult> UpdateRetweets(
        [FromRoute()] TweetIdDto idDto,
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
    /// Update one Tweet
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateTweet(
        [FromRoute()] TweetIdDto idDto,
        [FromQuery()] TweetUpdateInput tweetUpdateDto
    )
    {
        try
        {
            await _service.UpdateTweet(idDto, tweetUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
