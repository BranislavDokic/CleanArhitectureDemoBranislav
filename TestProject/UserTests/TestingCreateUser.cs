//using Application.Dtos;
//using Application.Interfaces.Repositoryinterfaces;
//using Application.Users.UserCommand;
//using Domain;
//using FakeItEasy;
//using Microsoft.Extensions.Logging;

//namespace TestProject;

//public class TestingCreateUser
//{
//    private IGenericRepositoryInterface<User> _fakeUserRepository;
//    private ILogger<AddNewUserCommandhandler> _mockLogger;
//    private AddNewUserCommandhandler _handler;

//    [SetUp]
//    public void Setup()
//    {
//        _fakeUserRepository = A.Fake<IGenericRepositoryInterface<User>>();
//        _mockLogger = A.Fake<ILogger<AddNewUserCommandhandler>>();
//        _handler = new AddNewUserCommandhandler(_fakeUserRepository, _mockLogger);
//    }

//    [Test]
//    public async Task Handle_UserAlreadyExists_ReturnsFailure()
//    {
//        var existingUser = new UserDTO
//        {
//            UserName = "existingUser",
//            Password = "password123"
//        };

//        var newUserCommand = new AddNewUserCommand(existingUser);

//        A.CallTo(() => _fakeUserRepository.GetAllAsync()).Returns(Task.FromResult(new List<User>
//            {
//                new User { Id = Guid.NewGuid(), UserName = "existingUser", PasswordHash = "someHashedPassword" }
//            }));

//        var result = await _handler.Handle(newUserCommand, CancellationToken.None);

//        Assert.IsFalse(result.IsSuccess);
//        Assert.AreEqual("Username is already taken", result.ErrorMessage);
//    }

//    [Test]
//    public async Task Handle_UserAddedSuccessfully_ReturnsSuccess()
//    {
//        var newUser = new UserDTO
//        {
//            UserName = "newUser",
//            Password = "password123"
//        };

//        var newUserCommand = new AddNewUserCommand(newUser);

//        A.CallTo(() => _fakeUserRepository.GetAllAsync()).Returns(Task.FromResult(new List<User>()));

//        A.CallTo(() => _fakeUserRepository.AddAsync(A<User>.Ignored)).ReturnsLazily((User u) => new User
//        {
//            Id = Guid.NewGuid(),
//            UserName = u.UserName,
//            PasswordHash = u.PasswordHash 
//        });

//        var result = await _handler.Handle(newUserCommand, CancellationToken.None);

//        Assert.IsTrue(result.IsSuccess);
//        Assert.AreEqual("User created successfully", result.Message);
//        Assert.IsNotNull(result.Data.PasswordHash); 
//    }

//    [Test]
//    public async Task Handle_ExceptionThrown_ReturnsFailure()
//    {
//        var newUser = new UserDTO
//        {
//            UserName = "newUserWithException",
//            Password = "password123"
//        };

//        var newUserCommand = new AddNewUserCommand(newUser);

//        A.CallTo(() => _fakeUserRepository.GetAllAsync()).Throws<TaskCanceledException>();

//        var result = await _handler.Handle(newUserCommand, CancellationToken.None);

//        Assert.IsFalse(result.IsSuccess);
//        Assert.AreEqual("An error occurred: A task was canceled.", result.ErrorMessage);
//    }

//    [Test]
//    public async Task Handle_UsernameIsNull_ReturnsFailure()
//    {
//        var newUser = new UserDTO
//        {
//            UserName = null,
//            Password = "password123"
//        };

//        var newUserCommand = new AddNewUserCommand(newUser);

//        var result = await _handler.Handle(newUserCommand, CancellationToken.None);

//        Assert.IsFalse(result.IsSuccess);
//        Assert.AreEqual("Username cannot be null or empty.", result.ErrorMessage);
//    }
//}
