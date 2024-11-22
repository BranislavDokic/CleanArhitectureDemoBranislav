using Application.Users.UserQueris.UserLogin.Helpers;
using Infrastructure.Database;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Users.UserQueris.UserLogin
{
    public class UserLoginQuerihandler : IRequestHandler<UserLoginQueri, string>
    {
        private readonly FakeDatabas _fakedatabas;
        private readonly TokenHelper _tokenHelper;

        public UserLoginQuerihandler(FakeDatabas fakedatabas, TokenHelper tokenHelper)
        {
            _fakedatabas = fakedatabas;
            _tokenHelper = tokenHelper;
        }


        
        public Task<string> Handle(UserLoginQueri request, CancellationToken cancellationToken)
        {
            var user = _fakedatabas.Users.FirstOrDefault(user => user.UserName == request.LoginUser.UserName && user.Password == request.LoginUser.Password);
            if (user == null) 
            {
                throw new UnauthorizedAccessException("Invalid username or password");
            }

            string token = _tokenHelper.GeneretJwtToken(user);

            return Task.FromResult(token);

        }
    }
}
