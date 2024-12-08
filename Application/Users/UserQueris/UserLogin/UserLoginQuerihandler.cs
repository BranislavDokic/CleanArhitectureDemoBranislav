using Application.Interfaces.Repositoryinterfaces;
using Application.Users.UserQueris.UserLogin.Helpers;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.UserQueris.UserLogin
{
    public class UserLoginQuerihandler : IRequestHandler<UserLoginQueri, OperationResult<string>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;
        private readonly TokenHelper _tokenHelper;
        private readonly ILogger<UserLoginQuerihandler> _logger;

        public UserLoginQuerihandler(IGenericRepositoryInterface<User> userRepository, TokenHelper tokenHelper, ILogger<UserLoginQuerihandler> logger)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
            _logger = logger;
        }
        public async Task<OperationResult<string>> Handle(UserLoginQueri request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to login user with username: {Username}", request.LoginUser.UserName);

            try
            {
                var users = await _userRepository.GetAllAsync();

                var foundUser = users.FirstOrDefault(u => u.UserName == request.LoginUser.UserName);

                if (foundUser == null || !PasswordHelper.VerifyPassword(foundUser.PasswordHash, request.LoginUser.Password))
                {
                    _logger.LogWarning("Login failed for username: {Username}. Invalid username or password.", request.LoginUser.UserName);
                    return OperationResult<string>.Failure("Invalid username or password", "Login failed");
                }

                string token = _tokenHelper.GeneretJwtToken(foundUser);

                _logger.LogInformation("User '{Username}' logged in successfully.", request.LoginUser.UserName);
                return OperationResult<string>.Success(token, "Login successful");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while logging in user: {Username}", request.LoginUser.UserName);
                return OperationResult<string>.Failure($"An error occurred: An error occurred while logging in user: {request.LoginUser.UserName}", "Login failed");
            }
        }
    }
}
