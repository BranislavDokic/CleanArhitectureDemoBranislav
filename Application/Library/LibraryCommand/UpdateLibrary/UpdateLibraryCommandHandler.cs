using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryCommand.UpdateLibrary
{
    public class UpdateLibraryCommandHandler : IRequestHandler<UpdateLibraryCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public UpdateLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<OperationResult<bool>> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var library = await _libraryRepository.GetByIdAsync(request.LibraryId);

                if (library == null)
                {
                    return OperationResult<bool>.Failure($"Library with ID {request.LibraryId} not found.");
                }

                library.Name = request.NewName;

                await _libraryRepository.UpdateAsync(request.LibraryId, library);

                return OperationResult<bool>.Success(true, "Library updated successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
