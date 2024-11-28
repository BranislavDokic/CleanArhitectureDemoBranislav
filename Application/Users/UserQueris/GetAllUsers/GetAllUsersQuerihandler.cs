using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;

namespace Application.Users.UserQueris.GetAllUsers
{
    internal sealed class GetAllUsersQuerihandler : IRequestHandler<GetAllUsersQueri, List<User>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;

        public GetAllUsersQuerihandler(IGenericRepositoryInterface<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> Handle(GetAllUsersQueri request, CancellationToken cancellationToken)
        {
            var allUsers = await _userRepository.GetAllAsync();
            return allUsers;
        }
    }
}
