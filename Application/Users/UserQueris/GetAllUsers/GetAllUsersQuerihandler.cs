using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Users.UserQueris.GetAllUsers
{
    internal sealed class GetAllUsersQuerihandler : IRequestHandler<GetAllUsersQueri, OperationResult<List<User>>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;
        private readonly ILogger<GetAllUsersQuerihandler> _logger;

        public GetAllUsersQuerihandler(IGenericRepositoryInterface<User> userRepository, ILogger<GetAllUsersQuerihandler> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<OperationResult<List<User>>> Handle(GetAllUsersQueri request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve all users.");

            try
            {
                var allUsers = await _userRepository.GetAllAsync();

                if (allUsers == null || allUsers.Count == 0)
                {
                    _logger.LogWarning("No users found.");
                    return OperationResult<List<User>>.Failure("No users found.");
                }

                _logger.LogInformation("{UserCount} users retrieved successfully.", allUsers.Count);
                return OperationResult<List<User>>.Success(allUsers, "Users retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return OperationResult<List<User>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
