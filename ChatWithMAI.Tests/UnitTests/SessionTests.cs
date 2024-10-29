using ChatWithMAI.Models;
using FluentAssertions;

namespace ChatWithMAI.Tests.UnitTests;

public class SessionTests
{
    [Fact]
    public void NewSession_ReturnsNullExceptionIfUserIsNull()
    {
        //Arrange
        var sessionConstruction = () => new Session(null!);
        //Act

        //Assert
        sessionConstruction.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void NewSessionWithUser_HasThisUser()
    {
        //Arrange
        var user = new User("User1","");
        var session = new Session(user);
        //Act
        var sessionHasUser = session.HasUser(user);
        //Assert
        sessionHasUser.Should().BeTrue();
    }
    
    [Fact]
    public void NewSessionWithUserAndUser2_SecondUserIsNullThrowsArgumentNullException()
    {
        //Arrange
        var user = new User("User1","");
        var session = new Session(user);
        //Act
        var sessionHasUser2 = () => session.WithUser2(null!);
        //Assert
        sessionHasUser2.Should().Throw<ArgumentNullException>();
    }
    
    [Fact]
    public void NewSessionWithUserAndUser2_HasUserAndUser2()
    {
        //Arrange
        var user = new User("User1","");
        var user2 = new User("User2","");
        var session = new Session(user).WithUser2(user2);
        //Act
        var sessionHasUser = session.HasUser(user);
        var sessionHasUser2 = session.HasUser(user2);
        //Assert
        sessionHasUser.Should().BeTrue();
        sessionHasUser2.Should().BeTrue();
    }
    
    [Fact]
    public void NewSessionWithUserAndAi_HasUserAndHasAiIsTrue()
    {
        //Arrange
        var user = new User("User1","");
        var session = new Session(user).WithAi();
        //Act
        var sessionHasUser = session.HasUser(user);
        var hasAi = session.HasAi;
        //Assert
        sessionHasUser.Should().BeTrue();
        hasAi.Should().BeTrue();
    }
}