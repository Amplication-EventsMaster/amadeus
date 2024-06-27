using Microsoft.AspNetCore.Mvc;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;

namespace TwitterClone.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class FollowersControllerBase : ControllerBase
{
    protected readonly IFollowersService _service;

    public FollowersControllerBase(IFollowersService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Follower
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<FollowerDto>> CreateFollower(FollowerCreateInput input)
    {
        var follower = await _service.CreateFollower(input);

        return CreatedAtAction(nameof(Follower), new { id = follower.Id }, follower);
    }

    /// <summary>
    /// Delete one Follower
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteFollower([FromRoute()] FollowerIdDto idDto)
    {
        try
        {
            await _service.DeleteFollower(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Followers
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<FollowerDto>>> Followers(
        [FromQuery()] FollowerFindMany filter
    )
    {
        return Ok(await _service.Followers(filter));
    }

    /// <summary>
    /// Connect multiple Followers records to Follower
    /// </summary>
    [HttpPost("{Id}/followers")]
    public async Task<ActionResult> ConnectFollowers(
        [FromRoute()] FollowerIdDto idDto,
        [FromQuery()] FollowerIdDto[] followersId
    )
    {
        try
        {
            await _service.ConnectFollower(idDto, followersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Followers records from Follower
    /// </summary>
    [HttpDelete("{Id}/followers")]
    public async Task<ActionResult> DisconnectFollowers(
        [FromRoute()] FollowerIdDto idDto,
        [FromBody()] FollowerIdDto[] followersId
    )
    {
        try
        {
            await _service.DisconnectFollower(idDto, followersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Followers records for Follower
    /// </summary>
    [HttpGet("{Id}/followers")]
    public async Task<ActionResult<List<FollowerDto>>> FindFollowers(
        [FromRoute()] FollowerIdDto idDto,
        [FromQuery()] FollowerFindMany filter
    )
    {
        try
        {
            return Ok(await _service.FindFollower(idDto, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Get a Follower record for Follower
    /// </summary>
    [HttpGet("{Id}/followers")]
    public async Task<ActionResult<List<FollowerDto>>> GetFollower(
        [FromRoute()] FollowerIdDto idDto
    )
    {
        var follower = await _service.GetFollower(idDto);
        return Ok(follower);
    }

    /// <summary>
    /// Meta data about Follower records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> FollowersMeta(
        [FromQuery()] FollowerFindMany filter
    )
    {
        return Ok(await _service.FollowersMeta(filter));
    }

    /// <summary>
    /// Update multiple Followers records for Follower
    /// </summary>
    [HttpPatch("{Id}/followers")]
    public async Task<ActionResult> UpdateFollowers(
        [FromRoute()] FollowerIdDto idDto,
        [FromBody()] FollowerIdDto[] followersId
    )
    {
        try
        {
            await _service.UpdateFollower(idDto, followersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Get one Follower
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<FollowerDto>> Follower([FromRoute()] FollowerIdDto idDto)
    {
        try
        {
            return await _service.Follower(idDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Follower
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateFollower(
        [FromRoute()] FollowerIdDto idDto,
        [FromQuery()] FollowerUpdateInput followerUpdateDto
    )
    {
        try
        {
            await _service.UpdateFollower(idDto, followerUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
