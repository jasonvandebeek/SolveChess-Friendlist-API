using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SolveChess.API.IntegrationTests;
using SolveChess.DAL.Models;
using SolveChess.IntegrationTests.Helpers;
using System.Net;

namespace SolveChess.API.Controllers.Tests;

[TestClass]
public class FriendlistControllerTests
{

    private SolveChessWebApplicationFactory _factory = null!;
    private AppDbContext _dbContext = null!;

    private string _jwtToken = null!;

    [TestInitialize]
    public void TestInitialize()
    {
        _factory = new SolveChessWebApplicationFactory();

        var scope = _factory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        _jwtToken = JwtTokenHelper.GenerateTestToken("123");
    }

    [TestMethod]
    public async Task AddFriendTest_Returns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        //Act
        var response = await client.PostAsync("/friendlist/request/234", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNotNull(request);
    }

    [TestMethod]
    public async Task AddFriendTest_AlreadyHasIncomingRequestReturns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var requestModel = new RequestModel
        {
            UserId = "123",
            SenderId = "234"
        };

        _dbContext.Add(requestModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.PostAsync("/friendlist/request/234", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNull(request);

        var friendToLink = await _dbContext.Friend.FirstOrDefaultAsync(f => f.UserId == "123" && f.FriendId == "234");
        var friendFromLink = await _dbContext.Friend.FirstOrDefaultAsync(f => f.UserId == "234" && f.FriendId == "123");
        Assert.IsNotNull(friendToLink);
        Assert.IsNotNull(friendFromLink);
    }

    [TestMethod]
    public async Task AddFriendTest_UsersAreAlreadyFriendsReturns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var userModel = new FriendModel
        {
            UserId = "234",
            FriendId = "123"
        };

        var friendModel = new FriendModel
        {
            UserId = "123",
            FriendId = "234"
        };

        await _dbContext.AddRangeAsync(userModel, friendModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.PostAsync("/friendlist/request/234", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNull(request);
    }

    [TestMethod]
    public async Task AddFriendTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.PostAsync("/friendlist/request/234", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task AcceptRequestTest_Returns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var requestModel = new RequestModel
        {
            UserId = "123",
            SenderId = "234"
        };

        _dbContext.Add(requestModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.PostAsync("/friendlist/request/234/accept", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNull(request);

        var friendToLink = await _dbContext.Friend.FirstOrDefaultAsync(f => f.UserId == "123" && f.FriendId == "234");
        var friendFromLink = await _dbContext.Friend.FirstOrDefaultAsync(f => f.UserId == "234" && f.FriendId == "123");
        Assert.IsNotNull(friendToLink);
        Assert.IsNotNull(friendFromLink);
    }

    [TestMethod]
    public async Task AcceptRequestTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.PostAsync("/friendlist/request/234/accept", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task DenyRequestTest_Returns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var requestModel = new RequestModel
        {
            UserId = "123",
            SenderId = "234"
        };

        _dbContext.Add(requestModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.PostAsync("/friendlist/request/234/deny", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNull(request);

        var friendToLink = await _dbContext.Friend.FirstOrDefaultAsync(f => f.UserId == "123" && f.FriendId == "234");
        var friendFromLink = await _dbContext.Friend.FirstOrDefaultAsync(f => f.UserId == "234" && f.FriendId == "123");
        Assert.IsNull(friendToLink);
        Assert.IsNull(friendFromLink);
    }

    [TestMethod]
    public async Task DenyRequestTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.PostAsync("/friendlist/request/234/deny", null);

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task RevokeRequestTest_Returns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var requestModel = new RequestModel
        {
            UserId = "123",
            SenderId = "234"
        };

        _dbContext.Add(requestModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.DeleteAsync("/friendlist/request/234");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNull(request);
    }

    [TestMethod]
    public async Task RevokeRequestTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.DeleteAsync("/friendlist/request/234");

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task RemoveFriendTest_Returns200Ok()
    {
        //Arrange
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var userModel = new FriendModel
        {
            UserId = "234",
            FriendId = "123"
        };

        var friendModel = new FriendModel
        {
            UserId = "123",
            FriendId = "234"
        };

        await _dbContext.AddRangeAsync(userModel, friendModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.DeleteAsync("/friendlist/234");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var request = await _dbContext.Request.FirstOrDefaultAsync(r => r.UserId == "234" && r.SenderId == "123");
        Assert.IsNull(request);
    }

    [TestMethod]
    public async Task RemoveFriendTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.DeleteAsync("/friendlist/234");

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task GetFriendlistTest_Returns200OkAndFriendlist()
    {
        //Arrange
        var json = new[]
        {
            "234"
        };
        var expected = JsonConvert.SerializeObject(json, Formatting.None);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var userModel = new FriendModel
        {
            UserId = "234",
            FriendId = "123"
        };

        var friendModel = new FriendModel
        {
            UserId = "123",
            FriendId = "234"
        };

        await _dbContext.AddRangeAsync(userModel, friendModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.GetAsync("/friendlist");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        Assert.AreEqual(expected, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task GetFriendlistTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync("/friendlist");

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task GetOutgoingRequestsTest_Returns200OkAndOutgoingRequestsList()
    {
        //Arrange
        var json = new[]
        {
            "234"
        };
        var expected = JsonConvert.SerializeObject(json, Formatting.None);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var requestModel = new RequestModel
        {
            UserId = "234",
            SenderId = "123"
        };

        _dbContext.Add(requestModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.GetAsync("/friendlist/requests/outgoing");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        Assert.AreEqual(expected, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task GetOutgoingRequestsTest_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync("/friendlist/requests/outgoing");

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task GetFriendRequests_Returns200OkAndFriendRequestsList()
    {
        //Arrange
        var json = new[]
        {
            "234"
        };
        var expected = JsonConvert.SerializeObject(json, Formatting.None);

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Cookie", $"AccessToken={_jwtToken}");

        var requestModel = new RequestModel
        {
            UserId = "123",
            SenderId = "234"
        };

        _dbContext.Add(requestModel);
        await _dbContext.SaveChangesAsync();

        //Act
        var response = await client.GetAsync("/friendlist/requests/incoming");

        //Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        Assert.AreEqual(expected, await response.Content.ReadAsStringAsync());
    }

    [TestMethod]
    public async Task GetFriendRequests_Returns401Unauthorized()
    {
        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync("/friendlist/requests/incoming");

        //Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestCleanup]
    public void TestCleanup()
    {
        var friendsToRemove = _dbContext.Friend.ToList();
        var requestsToRemove = _dbContext.Request.ToList();

        if(friendsToRemove.Count > 0 ) _dbContext.Friend.RemoveRange(friendsToRemove); 
        if (requestsToRemove.Count > 0) _dbContext.Request.RemoveRange(requestsToRemove);

        _dbContext.SaveChanges();
    }

}
