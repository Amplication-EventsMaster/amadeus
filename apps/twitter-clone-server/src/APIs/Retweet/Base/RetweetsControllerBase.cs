using Microsoft.AspNetCore.Mvc;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;

namespace TwitterClone.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class RetweetsControllerBase : ControllerBase
{
    protected readonly IRetweetsService _service;

    public RetweetsControllerBase(IRetweetsService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Retweet
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<RetweetDto>> CreateRetweet(RetweetCreateInput input)
    {
        var retweet = await _service.CreateRetweet(input);

        return CreatedAtAction(nameof(Retweet), new { id = retweet.Id }, retweet);
    }

    /// <summary>
    /// Delete one Retweet
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteRetweet([FromRoute()] RetweetIdDto idDto)
    {
        try
        {
            await _service.DeleteRetweet(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Retweets
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<RetweetDto>>> Retweets([FromQuery()] RetweetFindMany filter)
    {
        return Ok(await _service.Retweets(filter));
    }

    /// <summary>
    /// Get one Retweet
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<RetweetDto>> Retweet([FromRoute()] RetweetIdDto idDto)
    {
        try
        {
            return await _service.Retweet(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get a Tweet record for Retweet
    /// </summary>
    [HttpGet("{Id}/tweets")]
    public async Task<ActionResult<List<TweetDto>>> GetTweet([FromRoute()] RetweetIdDto idDto)
    {
        var tweet = await _service.GetTweet(idDto);
        return Ok(tweet);
    }

    /// <summary>
    /// Get a User record for Retweet
    /// </summary>
    [HttpGet("{Id}/users")]
    public async Task<ActionResult<List<UserDto>>> GetUser([FromRoute()] RetweetIdDto idDto)
    {
        var user = await _service.GetUser(idDto);
        return Ok(user);
    }

    /// <summary>
    /// Meta data about Retweet records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> RetweetsMeta([FromQuery()] RetweetFindMany filter)
    {
        return Ok(await _service.RetweetsMeta(filter));
    }

    /// <summary>
    /// Update one Retweet
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateRetweet(
        [FromRoute()] RetweetIdDto idDto,
        [FromQuery()] RetweetUpdateInput retweetUpdateDto
    )
    {
        try
        {
            await _service.UpdateRetweet(idDto, retweetUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
