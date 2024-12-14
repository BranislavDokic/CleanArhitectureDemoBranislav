using Application.Interfaces.Repositoryinterfaces;
using Application.Users.UserQueris.UserLogin.Helpers;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;


namespace Application.Users.UserCommand
{
    public class AddNewUserCommandhandler : IRequestHandler<AddNewUserCommand, OperationResult<IdentityResult>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AddNewUserCommandhandler> _logger;

        public AddNewUserCommandhandler(UserManager<User> userManager, ILogger<AddNewUserCommandhandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<OperationResult<IdentityResult>> Handle(AddNewUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to create new user with username: {Username}", request.NewUser.UserName);

            try
            {
                if (string.IsNullOrEmpty(request.NewUser.UserName))
                {
                    _logger.LogWarning("Username is null or empty.");
                    return OperationResult<IdentityResult>.Failure("Username cannot be null or empty.", "User creation failed");
                }

                var existingUser = await _userManager.FindByNameAsync(request.NewUser.UserName);

                if (existingUser != null)
                {
                    _logger.LogWarning("Username {Username} is already taken.", request.NewUser.UserName);
                    return OperationResult<IdentityResult>.Failure("Username is already taken", "User creation failed");
                }

                var user = new User
                {
                    UserName = request.NewUser.UserName
                };

                var result = await _userManager.CreateAsync(user, request.NewUser.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User '{Username}' created successfully.", request.NewUser.UserName);

                    if (!string.IsNullOrEmpty(request.NewUser.Role))
                    {
                        var roleResult = await _userManager.AddToRoleAsync(user, request.NewUser.Role);

                        if (!roleResult.Succeeded)
                        {
                            _logger.LogWarning("Failed to assign role '{Role}' to user '{Username}': {Errors}",
                                request.NewUser.Role, request.NewUser.UserName, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                            return OperationResult<IdentityResult>.Failure("User created, but failed to assign role.",
                                string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                        }

                        _logger.LogInformation("Role '{Role}' assigned to user '{Username}' successfully.",
                            request.NewUser.Role, request.NewUser.UserName);
                    }

                    return OperationResult<IdentityResult>.Success(result, "User created successfully");
                }
                else
                {
                    _logger.LogError("Failed to create user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return OperationResult<IdentityResult>.Failure("User creation failed.", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating user: {Username}", request.NewUser.UserName);
                return OperationResult<IdentityResult>.Failure($"An error occurred: {ex.Message}", "User creation failed");
            }
        }
    }
}
