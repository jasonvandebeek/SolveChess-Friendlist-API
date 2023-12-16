
namespace SolveChess.Logic.DAL;

public interface IFriendlistDal
{

    public Task AddFriendRequest(string userId, string targetUserId);
    public Task RemoveFriendRequest(string userId, string targetUserId);
    public Task<bool> HasFriendRequest(string userId, string targetUserId);
    public Task<bool> UsersAreFriends(string userId, string targetUserId);
    public Task AddFriend(string userId, string targetUserId);
    public Task RemoveFriend(string userId, string targetUserId);
    public Task<IEnumerable<string>> GetFriendlist(string userId);
    public Task<IEnumerable<string>> GetAllFriendRequests(string userId);
    public Task<IEnumerable<string>> GetOutgoingRequests(string userId);

}
