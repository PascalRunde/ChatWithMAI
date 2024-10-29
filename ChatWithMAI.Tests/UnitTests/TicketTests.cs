using ChatWithMAI.Models;
using FluentAssertions;

namespace ChatWithMAI.Tests.UnitTests;

public class TicketTests
{
    [Fact]
    public void CreateTicket_WithUserNullThrowsArgumentNullException()
    {
        //Arrange
        var ticketCreation = () => new Ticket(null!);
        
        //Assert
        ticketCreation.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void CreateTicket_WithUser_HasUser()
    {
        //Arrange
        var user = new User("user1", "");
        var ticket = new Ticket(user);
        
        //Act
        var ticketUser = ticket.User;
        
        //Assert
        ticketUser.Should().Be(user);
    }
}