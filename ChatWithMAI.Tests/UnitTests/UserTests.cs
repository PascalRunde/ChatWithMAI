using ChatWithMAI.Models;
using FluentAssertions;

namespace ChatWithMAI.Tests.UnitTests;

public class UserTests
{
    [Fact]
    public void NewUser_WithNameNullThrowsArgumentNullException()
    {
        //Arrange
        var userCreation = () =>new User(null!, "");
        
        //Assert
        userCreation.Should().Throw<ArgumentNullException>();

    }
    
    [Fact]
    public void NewUser_UserNameIsSet()
    {
        //Arrange
        var user = new User("user1", "");
        
        //Act
        var name = user.Username;
        //Assert
        name.Should().Be("user1");

    }
    
    [Fact]
    public void NewUser_WithConnectionIdNullThrowsArgumentNullException()
    {
        //Arrange
        var userCreation = () =>new User("user1",null!);
        
        //Act
        
        //Assert
        userCreation.Should().Throw<ArgumentNullException>();

    }
    
    [Fact]
    public void NewUser_ConnectionIdIsSet()
    {
        //Arrange
        var user = new User("user1", "connectionId");
        
        //Act
        var name = user.ConnectionId;
        //Assert
        name.Should().Be("connectionId");

    }
    
    [Fact]
    public void NewUser_IsInitializedWithIsPlayingFalse()
    {
        //Arrange
        var user = new User("user1", "");
        
        //Act
        var isPlaying = user.IsPlaying;
        
        //Assert
        isPlaying.Should().BeFalse();

    }
}