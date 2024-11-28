using Application.Books.Commands.CreateBook;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommandhandler : IRequestHandler<AddNewUserCommand, User>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;

        public AddNewUserCommandhandler(IGenericRepositoryInterface<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            var userToAdd = new User
            {
                Id = Guid.NewGuid(),
                UserName = request.NewUser.UserName,
                Password = request.NewUser.Password
            };

            await _userRepository.AddAsync(userToAdd);

            return userToAdd;
        }
    }
}
