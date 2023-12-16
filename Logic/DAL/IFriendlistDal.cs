
namespace SolveChess.Logic.DAL;

public interface IFriendlistDal
{

    public Task AddFriendRequest(string userId, string friendUserId);
    public Task RemoveFriendRequest(string userId, string friendUserId);
    public Task<bool> HasFriendRequest(string userId, string friendUserId);
    public Task<bool> UsersAreFriends(string userId, string friendUserId);
    public Task AddFriend(string userId, string friendUserId);
    public Task RemoveFriend(string userId, string friendUserId);
    public Task<IEnumerable<string>> GetFriendlist(string userId);
    public Task<IEnumerable<string>> GetAllFriendRequests(string userId);
    public Task<IEnumerable<string>> GetOutgoingRequests(string userId);

}
