﻿
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

    public async Task AddFriend(string userId, string targetUserId)
    {
        var userModel = new FriendModel
        {
            UserId = userId,
            FriendId = targetUserId
        };

        var friendModel = new FriendModel
        {
            UserId = targetUserId,
            FriendId = userId
        };

        await _dbContext.Friend.AddRangeAsync(userModel, friendModel);
        await _dbContext.SaveChangesAsync();
    }

    public async Task AddFriendRequest(string userId, string senderId)
    {
        var request = new RequestModel
        {
            UserId = userId,
            SenderId = senderId
        };

        _dbContext.Request.Add(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task RemoveFriendRequest(string userId, string senderId)
    {
        var request = await _dbContext.Request.Where(f => f.UserId == userId && f.SenderId == senderId).FirstOrDefaultAsync();   

        if (request == null)
            return;

        _dbContext.Remove(request);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> HasFriendRequest(string userId, string senderId)
    {
        var request = await _dbContext.Request.Where(f => f.UserId == userId && f.SenderId == senderId).FirstOrDefaultAsync();

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

    public async Task RemoveFriend(string userId, string targetUserId)
    {
        var userModel = await _dbContext.Friend.Where(f => f.UserId == userId && f.FriendId == targetUserId).FirstOrDefaultAsync();
        var friendModel = await _dbContext.Friend.Where(f => f.UserId == targetUserId && f.FriendId == userId).FirstOrDefaultAsync();

        if (userModel != null) _dbContext.Remove(userModel);
        if (friendModel != null) _dbContext.Remove(friendModel);

        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<string>> GetOutgoingRequests(string userId)
    {
        return await _dbContext.Request
            .Where(r => r.SenderId == userId)
            .Select(r => r.UserId)
            .ToListAsync();
    }

    public async Task<bool> UsersAreFriends(string userId, string targetUserId)
    {
        var request = await _dbContext.Friend.Where(f => f.UserId == userId && f.FriendId == targetUserId).FirstOrDefaultAsync();

        return request != null;
    }
}

