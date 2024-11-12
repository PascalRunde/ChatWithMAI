using ChatWithMAI.Models;
using FluentAssertions;

namespace ChatWithMAI.Tests.IntegrationTests;

using Services;

public class UserMatchinServiceTest
{
    [Fact]
    public async Task UserMatchingService_RunsFor10Seconds_WhenNoOtherUsersConnect()
    {
        //Arrange
        var Ticket = new Ticket(new User("user1", ""));
        // UserMatchingService ums = new UserMatchingService();

        //Act
        // await ums.AddUserToSearch(ticket,);

        //Assert
        Ticket.IsRedeemed.Should().BeTrue();
    }
}
