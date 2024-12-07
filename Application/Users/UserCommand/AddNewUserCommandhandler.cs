using Application.Books.Commands.CreateBook;
using Application.Interfaces.Repositoryinterfaces;
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
            _logger.LogInformation("Attempting to add a new user with username: {Username}", request.NewUser.UserName);

            try
            {
                var existingUsers = await _userRepository.GetAllAsync();

                var userAlreadyExists = existingUsers.FirstOrDefault(u => u.UserName.Equals(request.NewUser.UserName, StringComparison.OrdinalIgnoreCase));

                if (userAlreadyExists != null)
                {
                    _logger.LogWarning("Username '{Username}' is already taken.", request.NewUser.UserName);
                    return OperationResult<User>.Failure($"Username '{request.NewUser.UserName}' is already taken.", "Failed to add the user.");
                }

                var userToAdd = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.NewUser.UserName,
                    Password = request.NewUser.Password
                };

                await _userRepository.AddAsync(userToAdd);

                _logger.LogInformation("User '{Username}' has been successfully added.", request.NewUser.UserName);
                return OperationResult<User>.Success(userToAdd, "User has been successfully added.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding the user: {Username}", request.NewUser.UserName);
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}", "Failed to add the user.");
            }
        }
    }
}
