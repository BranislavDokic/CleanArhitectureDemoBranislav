using Application.Interfaces.Repositoryinterfaces;
using Application.Users.UserQueris.UserLogin.Helpers;
using Domain;
using MediatR;

namespace Application.Users.UserQueris.UserLogin
{
    public class UserLoginQuerihandler : IRequestHandler<UserLoginQueri, string>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;
        private readonly TokenHelper _tokenHelper;

        public UserLoginQuerihandler(IGenericRepositoryInterface<User> userRepository, TokenHelper tokenHelper)
        {
            _userRepository = userRepository;
            _tokenHelper = tokenHelper;
        }



        public async Task<string> Handle(UserLoginQueri request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetAllAsync();
            var foundUser = user.FirstOrDefault(u => u.UserName == request.LoginUser.UserName && u.Password == request.LoginUser.Password);

            if (foundUser == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            string token = _tokenHelper.GeneretJwtToken(foundUser);

            return token;
        }
    }
}
