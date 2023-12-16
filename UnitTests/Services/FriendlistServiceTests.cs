
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SolveChess.Logic.DAL;

namespace SolveChess.Logic.Services.Tests;

[TestClass]
public class FriendlistServiceTests
{

    [TestMethod]
    public async Task GetOutgoingRequestsTest()
    {
        //Arrange 
        var expected = new List<string>() { "123", "124", "125" };

        var friendlistDalMock = new Mock<IFriendlistDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        friendlistDalMock.Setup(dal => dal.GetOutgoingRequests(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new FriendlistService(friendlistDalMock.Object, httpClient);

        //Act
        var result = await service.GetOutgoingRequests("100");

        //Assert
        CollectionAssert.AreEquivalent(expected, result.ToList());
    }

    [TestMethod]
    public async Task GetFriendlistTest()
    {
        //Arrange 
        var expected = new List<string>() { "123", "124", "125" };

        var friendlistDalMock = new Mock<IFriendlistDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        friendlistDalMock.Setup(dal => dal.GetFriendlist(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new FriendlistService(friendlistDalMock.Object, httpClient);

        //Act
        var result = await service.GetFriendlist("100");

        //Assert
        CollectionAssert.AreEquivalent(expected, result.ToList());
    }

    [TestMethod]
    public async Task GetAllFriendRequestsTest()
    {
        //Arrange 
        var expected = new List<string>() { "123", "124", "125" };

        var friendlistDalMock = new Mock<IFriendlistDal>();
        var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        var httpClient = new HttpClient(httpMessageHandlerMock.Object);

        friendlistDalMock.Setup(dal => dal.GetAllFriendRequests(It.IsAny<string>()))
            .ReturnsAsync(expected);

        var service = new FriendlistService(friendlistDalMock.Object, httpClient);

        //Act
        var result = await service.GetAllFriendRequests("100");

        //Assert
        CollectionAssert.AreEquivalent(expected, result.ToList());
    }

}
