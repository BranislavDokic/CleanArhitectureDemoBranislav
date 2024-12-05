using Application.Books.Commands.CreateBook;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommandhandler : IRequestHandler<AddNewUserCommand, OperationResult<User>>
    {
        private readonly IGenericRepositoryInterface<User> _userRepository;

        public AddNewUserCommandhandler(IGenericRepositoryInterface<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<OperationResult<User>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingUsers = await _userRepository.GetAllAsync();

                var userAlreadyExists = existingUsers.FirstOrDefault(u => u.UserName.Equals(request.NewUser.UserName, StringComparison.OrdinalIgnoreCase));

                if (userAlreadyExists != null)
                {
                    return OperationResult<User>.Failure($"Username '{request.NewUser.UserName}' is already taken.", "Failed to add the user.");
                }

                var userToAdd = new User
                {
                    Id = Guid.NewGuid(),
                    UserName = request.NewUser.UserName,
                    Password = request.NewUser.Password
                };

                await _userRepository.AddAsync(userToAdd);

                return OperationResult<User>.Success(userToAdd, "User has been successfully added.");
            }
            catch (Exception ex)
            {
                return OperationResult<User>.Failure($"An error occurred: {ex.Message}", "Failed to add the user.");
            }
        }
    }
}
