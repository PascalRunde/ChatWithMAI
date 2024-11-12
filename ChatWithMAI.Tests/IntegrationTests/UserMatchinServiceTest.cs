using ChatWithMAI.Models;
using FluentAssertions;

namespace ChatWithMAI.Tests.IntegrationTests;

using Moq;
using Services;

public class UserMatchinServiceTest
{
    private readonly Mock<SessionService> sessionServiceMock;
    public UserMatchinServiceTest()
    {
        sessionServiceMock = new Mock<SessionService>();
    }
    [Fact]
    public async Task UserMatchingService_RunsFor10Seconds_WhenNoOtherUsersConnect()
    {
        //Arrange
        var ticket = new Ticket(new User("user1", ""));
        var ums = new UserMatchingService(sessionServiceMock.Object);

        //Act
        await ums.AddUserToSearch(ticket, MockFunction);

        //Assert
        ticket.IsRedeemed.Should().BeTrue();
    }

    private Task MockFunction(User user) => Task.CompletedTask;
}
