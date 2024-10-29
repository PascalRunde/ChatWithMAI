using ChatWithMAI.Models;
using ChatWithMAI.Services;
namespace ChatWithMAI.Tests;

public class SessionServiceTests
{
    private readonly SessionService sessionService;
    public SessionServiceTests()
    {
        sessionService = new SessionService();
    }
    [Fact]
    public void CreateNewSession_ReturnsSession()
    {
        //Arrange
        
        sessionService.AssignToSession(new User("User1",""),new User("User2",""));

        //Act

        //Assert
    }
}