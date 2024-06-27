using Microsoft.EntityFrameworkCore;
using TwitterClone.APIs;
using TwitterClone.APIs.Common;
using TwitterClone.APIs.Dtos;
using TwitterClone.APIs.Errors;
using TwitterClone.APIs.Extensions;
using TwitterClone.Infrastructure;
using TwitterClone.Infrastructure.Models;

namespace TwitterClone.APIs;

public abstract class FollowersServiceBase : IFollowersService
{
    protected readonly TwitterCloneDbContext _context;

    public FollowersServiceBase(TwitterCloneDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Follower
    /// </summary>
    public async Task<FollowerDto> CreateFollower(FollowerCreateInput createDto)
    {
        var follower = new Follower
        {
            CreatedAt = createDto.CreatedAt,
            Following = createDto.Following,
            UpdatedAt = createDto.UpdatedAt
        };

        if (createDto.Id != null)
        {
            follower.Id = createDto.Id;
        }
        if (createDto.Follower != null)
        {
            follower.Follower = await _context
                .Followers.Where(follower => createDto.Follower.Id == follower.Id)
                .FirstOrDefaultAsync();
        }

        _context.Followers.Add(follower);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<Follower>(follower.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Follower
    /// </summary>
    public async Task DeleteFollower(FollowerIdDto idDto)
    {
        var follower = await _context.Followers.FindAsync(idDto.Id);
        if (follower == null)
        {
            throw new NotFoundException();
        }

        _context.Followers.Remove(follower);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Followers
    /// </summary>
    public async Task<List<FollowerDto>> Followers(FollowerFindMany findManyArgs)
    {
        var followers = await _context
            .Followers.Include(x => x.Follower)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return followers.ConvertAll(follower => follower.ToDto());
    }

    /// <summary>
    /// Connect multiple Followers records to Follower
    /// </summary>
    public async Task ConnectFollowers(FollowerIdDto idDto, FollowerIdDto[] followersId)
    {
        var follower = await _context
            .Followers.Include(x => x.Follower)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (follower == null)
        {
            throw new NotFoundException();
        }

        var followers = await _context
            .Followers.Where(t => followersId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (followers.Count == 0)
        {
            throw new NotFoundException();
        }

        var followersToConnect = followers.Except(follower.Follower);

        foreach (var follower in followersToConnect)
        {
            follower.Follower.Add(follower);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Followers records from Follower
    /// </summary>
    public async Task DisconnectFollowers(FollowerIdDto idDto, FollowerIdDto[] followersId)
    {
        var follower = await _context
            .Followers.Include(x => x.Follower)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (follower == null)
        {
            throw new NotFoundException();
        }

        var followers = await _context
            .Followers.Where(t => followersId.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var follower in followers)
        {
            follower.Follower?.Remove(follower);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Followers records for Follower
    /// </summary>
    public async Task<List<FollowerDto>> FindFollowers(
        FollowerIdDto idDto,
        FollowerFindMany followerFindMany
    )
    {
        var followers = await _context
            .Followers.Where(m => m.FollowerId == idDto.Id)
            .ApplyWhere(followerFindMany.Where)
            .ApplySkip(followerFindMany.Skip)
            .ApplyTake(followerFindMany.Take)
            .ApplyOrderBy(followerFindMany.SortBy)
            .ToListAsync();

        return followers.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Get a Follower record for Follower
    /// </summary>
    public async Task<FollowerDto> GetFollower(FollowerIdDto idDto)
    {
        var follower = await _context
            .Followers.Where(follower => follower.Id == idDto.Id)
            .Include(follower => follower.Follower)
            .FirstOrDefaultAsync();
        if (follower == null)
        {
            throw new NotFoundException();
        }
        return follower.Follower.ToDto();
    }

    /// <summary>
    /// Meta data about Follower records
    /// </summary>
    public async Task<MetadataDto> FollowersMeta(FollowerFindMany findManyArgs)
    {
        var count = await _context.Followers.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Update multiple Followers records for Follower
    /// </summary>
    public async Task UpdateFollowers(FollowerIdDto idDto, FollowerIdDto[] followersId)
    {
        var follower = await _context
            .Followers.Include(t => t.Follower)
            .FirstOrDefaultAsync(x => x.Id == idDto.Id);
        if (follower == null)
        {
            throw new NotFoundException();
        }

        var followers = await _context
            .Followers.Where(a => followersId.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (followers.Count == 0)
        {
            throw new NotFoundException();
        }

        follower.Follower = followers;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Get one Follower
    /// </summary>
    public async Task<FollowerDto> Follower(FollowerIdDto idDto)
    {
        var followers = await this.Followers(
            new FollowerFindMany { Where = new FollowerWhereInput { Id = idDto.Id } }
        );
        var follower = followers.FirstOrDefault();
        if (follower == null)
        {
            throw new NotFoundException();
        }

        return follower;
    }

    /// <summary>
    /// Update one Follower
    /// </summary>
    public async Task UpdateFollower(FollowerIdDto idDto, FollowerUpdateInput updateDto)
    {
        var follower = updateDto.ToModel(idDto);

        _context.Entry(follower).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Followers.Any(e => e.Id == follower.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }
}
