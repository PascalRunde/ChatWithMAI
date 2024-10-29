using ChatWithMAI.Models;
using FluentAssertions;

namespace ChatWithMAI.Tests.IntegrationTests;

public class TicketTests
{
    [Fact]
    public async Task TicketRun_RunsFor10Seconds()
    {
        //Arrange
        var Ticket = new Ticket(new User("user1", ""));

        //Act
        await Ticket.Run();
        
        //Assert
        Ticket.IsRedeemed.Should().BeTrue();
    }
}