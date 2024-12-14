//using Application.Interfaces.Repositoryinterfaces;
//using Application.Users.UserQueris.GetAllUsers;
//using Domain;
//using FakeItEasy;
//using Microsoft.Extensions.Logging;

//namespace TestProject;

//public class TestingGetAllUsers
//{
//    private IGenericRepositoryInterface<User> _fakeUserRepository;
//    private ILogger<GetAllUsersQuerihandler> _fakeLogger;
//    private GetAllUsersQuerihandler _handler;

//    [SetUp]
//    public void SetUp()
//    {
//        _fakeUserRepository = A.Fake<IGenericRepositoryInterface<User>>();
//        _fakeLogger = A.Fake<ILogger<GetAllUsersQuerihandler>>();
//        _handler = new GetAllUsersQuerihandler(_fakeUserRepository, _fakeLogger);
//    }

//    [Test]
//    public async Task Handle_WhenNoUsersFound_ReturnsFailure()
//    {
//        A.CallTo(() => _fakeUserRepository.GetAllAsync()).Returns(Task.FromResult(new List<User>()));

//        var query = new GetAllUsersQueri();

//        var result = await _handler.Handle(query, CancellationToken.None);

//        Assert.IsFalse(result.IsSuccess);
//        Assert.AreEqual("No users found.", result.ErrorMessage);
//    }

//    [Test]
//    public async Task Handle_WhenUsersFound_ReturnsSuccess()
//    {
//        var users = new List<User>
//        {
//            new User { Id = Guid.NewGuid(), UserName = "user1", PasswordHash = "hashedPassword1" },
//            new User { Id = Guid.NewGuid(), UserName = "user2", PasswordHash = "hashedPassword2" }
//        };

//        A.CallTo(() => _fakeUserRepository.GetAllAsync()).Returns(Task.FromResult(users));

//        var query = new GetAllUsersQueri();

//        var result = await _handler.Handle(query, CancellationToken.None);

//        Assert.IsTrue(result.IsSuccess);
//        Assert.AreEqual("Users retrieved successfully.", result.Message);
//        Assert.AreEqual(2, result.Data.Count);

//        Assert.IsNotNull(result.Data[0].PasswordHash);
//        Assert.IsNotNull(result.Data[1].PasswordHash);
//    }

//}
