using Microsoft.AspNetCore.Mvc;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;

namespace TwitterClone.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class LikesControllerBase : ControllerBase
{
    protected readonly ILikesService _service;

    public LikesControllerBase(ILikesService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Like
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<LikeDto>> CreateLike(LikeCreateInput input)
    {
        var like = await _service.CreateLike(input);

        return CreatedAtAction(nameof(Like), new { id = like.Id }, like);
    }

    /// <summary>
    /// Delete one Like
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteLike([FromRoute()] LikeIdDto idDto)
    {
        try
        {
            await _service.DeleteLike(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Likes
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<LikeDto>>> Likes([FromQuery()] LikeFindMany filter)
    {
        return Ok(await _service.Likes(filter));
    }

    /// <summary>
    /// Get one Like
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<LikeDto>> Like([FromRoute()] LikeIdDto idDto)
    {
        try
        {
            return await _service.Like(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get a Tweet record for Like
    /// </summary>
    [HttpGet("{Id}/tweets")]
    public async Task<ActionResult<List<TweetDto>>> GetTweet([FromRoute()] LikeIdDto idDto)
    {
        var tweet = await _service.GetTweet(idDto);
        return Ok(tweet);
    }

    /// <summary>
    /// Get a User record for Like
    /// </summary>
    [HttpGet("{Id}/users")]
    public async Task<ActionResult<List<UserDto>>> GetUser([FromRoute()] LikeIdDto idDto)
    {
        var user = await _service.GetUser(idDto);
        return Ok(user);
    }

    /// <summary>
    /// Meta data about Like records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> LikesMeta([FromQuery()] LikeFindMany filter)
    {
        return Ok(await _service.LikesMeta(filter));
    }

    /// <summary>
    /// Update one Like
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateLike(
        [FromRoute()] LikeIdDto idDto,
        [FromQuery()] LikeUpdateInput likeUpdateDto
    )
    {
        try
        {
            await _service.UpdateLike(idDto, likeUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
