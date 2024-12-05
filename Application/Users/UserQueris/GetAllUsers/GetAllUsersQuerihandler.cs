using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;

namespace Application.Users.UserQueris.GetAllUsers
{
    internal sealed class GetAllUsersQuerihandler : IRequestHandler<GetAllUsersQueri, OperationResult<List<User>>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;

        public GetAllUsersQuerihandler(IGenericRepositoryInterface<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQueri request, CancellationToken cancellationToken)
        {
            try
            {
                var allUsers = await _userRepository.GetAllAsync();

                if (allUsers == null || allUsers.Count == 0)
                {
                    return OperationResult<List<User>>.Failure("No users found.");
                }

                return OperationResult<List<User>>.Success(allUsers, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<User>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
