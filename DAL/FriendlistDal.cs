
using Microsoft.EntityFrameworkCore;
using SolveChess.DAL.Models;
using SolveChess.Logic.DAL;

namespace SolveChess.DAL;

public class FriendlistDal : IFriendlistDal
{

    private readonly AppDbContext _dbContext;

    public FriendlistDal(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddFriend(string userId, string friendUserId)
    {
        var userModel = new FriendModel
        {
            UserId = userId,
            FriendId = friendUserId
        };

        var friendModel = new FriendModel
        {
            UserId = friendUserId,
            FriendId = userId
        };

        await _dbContext.AddRangeAsync(userModel, friendModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddFriendRequest(string userId, string friendUserId)
    {
        var request = new RequestModel
        {
            UserId = friendUserId,
            SenderId = userId
        };

        _dbContext.Add(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriendRequest(string userId, string friendUserId)
    {
        var request = await _dbContext.Friend.Where(f => f.UserId == userId && f.FriendId == friendUserId).FirstOrDefaultAsync();   

        if (request == null)
            return;

        _dbContext.Remove(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> HasFriendRequest(string userId, string friendUserId)
    {
        var request = await _dbContext.Request.Where(f => f.UserId == userId && f.SenderId == friendUserId).FirstOrDefaultAsync();

        return request != null;
    }

    public async Task<IEnumerable<string>> GetAllFriendRequests(string userId)
    {
        return await _dbContext.Request
            .Where(r => r.UserId == userId)
            .Select(r => r.SenderId)
            .ToListAsync();
    }

    public async Task<IEnumerable<string>> GetFriendlist(string userId)
    {
        return await _dbContext.Friend
            .Where(f => f.UserId == userId)
            .Select(f => f.FriendId)
            .ToListAsync();
    }

    public async Task RemoveFriend(string userId, string friendUserId)
    {
        var userModel = await _dbContext.Friend.Where(f => f.UserId == userId && f.FriendId == friendUserId).FirstOrDefaultAsync();
        var friendModel = await _dbContext.Friend.Where(f => f.UserId == friendUserId && f.FriendId == userId).FirstOrDefaultAsync();

        if (userModel == null || friendModel == null)
            return;

        _dbContext.RemoveRange(userModel, friendModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetOutgoingRequests(string userId)
    {
        return await _dbContext.Request
            .Where(r => r.SenderId == userId)
            .Select(r => r.UserId)
            .ToListAsync();
    }

    public async Task<bool> UsersAreFriends(string userId, string friendUserId)
    {
        var request = await _dbContext.Friend.Where(f => f.UserId == userId && f.FriendId == friendUserId).FirstOrDefaultAsync();

        return request != null;
    }
}

