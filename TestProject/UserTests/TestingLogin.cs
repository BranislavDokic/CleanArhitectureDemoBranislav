//using Application.Dtos;
//using Application.Interfaces.Repositoryinterfaces;
//using Application.Users.UserQueris.UserLogin.Helpers;
//using Application.Users.UserQueris.UserLogin;
//using Domain;
//using FakeItEasy;
//using Microsoft.Extensions.Logging;

//namespace TestProject;

//public class TestingLogin
//{
//    private IGenericRepositoryInterface<User> _fakeUserRepository;
//    private TokenHelper _fakeTokenHelper;
//    private ILogger<UserLoginQuerihandler> _fakeLogger;
//    private UserLoginQuerihandler _handler;

//    [SetUp]
//    public void SetUp()
//    {
//        _fakeUserRepository = A.Fake<IGenericRepositoryInterface<User>>();
//        _fakeTokenHelper = A.Fake<TokenHelper>();
//        _fakeLogger = A.Fake<ILogger<UserLoginQuerihandler>>();

//        _handler = new UserLoginQuerihandler(_fakeUserRepository, _fakeTokenHelper, _fakeLogger);
//    }

    
//    [Test]
//    public async Task Handle_WhenUserDoesNotExist_ReturnsFailure()
//    {
//        var users = new List<User>(); 
//        var fakeTokenHelper = A.Fake<TokenHelper>(options => options.CallsBaseMethods()); 
//        var fakeUserRepository = A.Fake<IGenericRepositoryInterface<User>>();
//        A.CallTo(() => fakeUserRepository.GetAllAsync()).Returns(Task.FromResult(users));

//        var fakeLogger = A.Fake<ILogger<UserLoginQuerihandler>>();

//        var handler = new UserLoginQuerihandler(fakeUserRepository, fakeTokenHelper, fakeLogger);
//        var query = new UserLoginQueri(new UserDTO { UserName = "user1", Password = "password1" });

//        var result = await handler.Handle(query, CancellationToken.None);

//        Assert.IsFalse(result.IsSuccess);
//        Assert.AreEqual("Invalid username or password", result.ErrorMessage);
//    }

//    [Test]
//    public async Task Handle_WhenExceptionThrown_ReturnsFailure()
//    {
//        var fakeTokenHelper = A.Fake<TokenHelper>(options => options.CallsBaseMethods());
//        var fakeUserRepository = A.Fake<IGenericRepositoryInterface<User>>();
//        A.CallTo(() => fakeUserRepository.GetAllAsync()).Throws<Exception>();

//        var fakeLogger = A.Fake<ILogger<UserLoginQuerihandler>>();

//        var handler = new UserLoginQuerihandler(fakeUserRepository, fakeTokenHelper, fakeLogger);
//        var query = new UserLoginQueri(new UserDTO { UserName = "user1", Password = "password1" });

//        var result = await handler.Handle(query, CancellationToken.None);

//        Assert.IsFalse(result.IsSuccess);
//        Assert.AreEqual("An error occurred: An error occurred while logging in user: user1", result.ErrorMessage);
//    }
//}
