
namespace SolveChess.Logic.Interfaces;

public interface IFriendlistService
{

    public Task SendFriendRequest(string userId, string friendUserId);
    public Task RemoveFriend(string userId, string friendUserId);
    public Task<IEnumerable<string>> GetFriendlist(string userId);
    public Task<IEnumerable<string>> GetAllFriendRequests(string userId);
    public Task<IEnumerable<string>> GetOutgoingRequests(string userId);
    public Task AcceptFriendRequest(string userId, string friendUserId);
    public Task DenyFriendRequest(string userId, string friendUserId);
    public Task RevokeFriendRequest(string userId, string friendUserId);

}
