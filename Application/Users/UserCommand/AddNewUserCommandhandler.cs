using Application.Interfaces.Repositoryinterfaces;
using Application.Users.UserQueris.UserLogin.Helpers;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommandhandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;
        private readonly ILogger<AddNewUserCommandhandler> _logger;

        public AddNewUserCommandhandler(IGenericRepositoryInterface<User> userRepository, ILogger<AddNewUserCommandhandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create new user with username: {Username}", request.NewUser.UserName);

            try
            {
                if (string.IsNullOrEmpty(request.NewUser.UserName))
                {
                    _logger.LogWarning("Username is null or empty.");
                    return OperationResult<User>.Failure("Username cannot be null or empty.", "User creation failed");
                }

                var existingUser = await _userRepository.GetAllAsync();

                if (existingUser.Any(u => u.UserName == request.NewUser.UserName))
                {
                    _logger.LogWarning("Username {Username} is already taken.", request.NewUser.UserName);
                    return OperationResult<User>.Failure("Username is already taken", "User creation failed");
                }

                var hashedPassword = PasswordHelper.HashPassword(request.NewUser.Password);

                var user = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.NewUser.UserName,
                    PasswordHash = hashedPassword
                };

                await _userRepository.AddAsync(user);

                _logger.LogInformation("User '{Username}' created successfully.", request.NewUser.UserName);
                return OperationResult<User>.Success(user, "User created successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user: {Username}", request.NewUser.UserName);
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}", "User creation failed");
            }
        }
    }
}
