using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SolveChess.API.Exceptions;
using SolveChess.Logic.Interfaces;

namespace SolveChess.API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class FriendlistController : Controller
{

    private readonly IFriendlistService _friendlistService;

    public FriendlistController(IFriendlistService chessService)
    {
        _friendlistService = chessService;
    }

    [HttpPost("requests/{friendUserId}")]
    public async Task<IActionResult> AddFriend(string friendUserId)
    {
        var userId = GetUserIdFromCookies();

        await _friendlistService.SendFriendRequest(userId, friendUserId);

        return Ok();
    }

    [HttpPost("requests/{friendUserId}/accept")]
    public async Task<IActionResult> AcceptRequest(string friendUserId)
    {
        var userId = GetUserIdFromCookies();

        await _friendlistService.AcceptFriendRequest(userId, friendUserId);

        return Ok();
    }

    [HttpPost("requests/{friendUserId}/deny")]
    public async Task<IActionResult> DenyRequest(string friendUserId)
    {
        var userId = GetUserIdFromCookies();

        await _friendlistService.DenyFriendRequest(userId, friendUserId);

        return Ok();
    }

    [HttpDelete("requests/{friendUserId}")]
    public async Task<IActionResult> RevokeRequest(string friendUserId)
    {
        var userId = GetUserIdFromCookies();

        await _friendlistService.RevokeFriendRequest(userId, friendUserId);

        return Ok();
    }

    [HttpDelete("{friendUserId}")]
    public async Task<IActionResult> RemoveFriend(string friendUserId)
    {
        var userId = GetUserIdFromCookies();

        await _friendlistService.RemoveFriend(userId, friendUserId);

        return Ok();
    }

    [HttpGet()]
    public async Task<IActionResult> GetFriendlist()
    {
        var userId = GetUserIdFromCookies();

        var friends = await _friendlistService.GetFriendlist(userId);

        return Ok(friends);
    }

    [HttpGet("requests/outgoing")]
    public async Task<IActionResult> GetOutgoingRequests()
    {
        var userId = GetUserIdFromCookies();

        var requests = await _friendlistService.GetOutgoingRequests(userId);

        return Ok(requests);
    }

    [HttpGet("requests/incoming")]
    public async Task<IActionResult> GetFriendRequests()
    {
        var userId = GetUserIdFromCookies();

        var requests = await _friendlistService.GetAllFriendRequests(userId);

        return Ok(requests);
    }

    private string GetUserIdFromCookies()
    {
        var userId = HttpContext.User.FindFirst("Id")?.Value ?? throw new InvalidJwtTokenException();
        return userId;
    }

}

