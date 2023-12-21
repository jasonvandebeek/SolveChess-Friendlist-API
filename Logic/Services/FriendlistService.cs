
using SolveChess.Logic.DAL;
using SolveChess.Logic.Interfaces;

namespace SolveChess.Logic.Services;

public class FriendlistService : IFriendlistService
{

    private readonly IFriendlistDal _friendlistDal;
    private readonly HttpClient _httpClient;

    public FriendlistService(IFriendlistDal gameDal, HttpClient httpClient)
    {
        _friendlistDal = gameDal;
        _httpClient = httpClient;
    }

    public async Task AcceptFriendRequest(string userId, string friendUserId)
    {
        await _friendlistDal.RemoveFriendRequest(userId, friendUserId);
        await _friendlistDal.AddFriend(userId, friendUserId);
    }

    public async Task SendFriendRequest(string userId, string friendUserId)
    {
        var response = await _httpClient.GetAsync($"https://api.solvechess.xyz/auth/user/{friendUserId}");
        if (!response.IsSuccessStatusCode)
            return;

        if (await _friendlistDal.UsersAreFriends(userId, friendUserId))
            return;

        if(await _friendlistDal.HasFriendRequest(userId, friendUserId))
        {
            await AcceptFriendRequest(userId, friendUserId);
            return;
        }

        if (await _friendlistDal.HasFriendRequest(friendUserId, userId))
            return;

        await _friendlistDal.AddFriendRequest(userId, friendUserId);
    }

    public async Task DenyFriendRequest(string userId, string friendUserId)
    {
        await _friendlistDal.RemoveFriendRequest(userId, friendUserId);
    }

    public async Task<IEnumerable<string>> GetAllFriendRequests(string userId)
    {
        return await _friendlistDal.GetAllFriendRequests(userId);
    }

    public async Task<IEnumerable<string>> GetFriendlist(string userId)
    {
        return await _friendlistDal.GetFriendlist(userId);
    }

    public async Task<IEnumerable<string>> GetOutgoingRequests(string userId)
    {
        return await _friendlistDal.GetOutgoingRequests(userId);
    }

    public async Task RemoveFriend(string userId, string friendUserId)
    {
        await _friendlistDal.RemoveFriend(userId, friendUserId);
    }

    public async Task RevokeFriendRequest(string userId, string friendUserId)
    {
        await _friendlistDal.RemoveFriendRequest(friendUserId, userId);
    }
}