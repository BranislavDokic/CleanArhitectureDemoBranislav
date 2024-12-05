using Application.Interfaces.Repositoryinterfaces;
using Application.Users.UserQueris.UserLogin.Helpers;
using Domain;
using Domain.Result;
using MediatR;

namespace Application.Users.UserQueris.UserLogin
{
    public class UserLoginQuerihandler : IRequestHandler<UserLoginQueri, OperationResult<string>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;
        private readonly TokenHelper _tokenHelper;

        public UserLoginQuerihandler(IGenericRepositoryInterface<User> userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }



        public async Task<OperationResult<string>> Handle(UserLoginQueri request, CancellationToken cancellationToken)
        {
            try
            {
                var users = await _userRepository.GetAllAsync();

                var foundUser = users.FirstOrDefault(u => u.UserName == request.LoginUser.UserName && u.Password == request.LoginUser.Password);

                if (foundUser == null)
                {
                    return OperationResult<string>.Failure("Invalid username or password", "Login failed");
                }

                string token = _tokenHelper.GeneretJwtToken(foundUser);

                return OperationResult<string>.Success(token, "Login successful");
            }
            catch (Exception ex)
            {
                return OperationResult<string>.Failure($"An error occurred: {ex.Message}", "Login failed");
            }
        }
    }
}
