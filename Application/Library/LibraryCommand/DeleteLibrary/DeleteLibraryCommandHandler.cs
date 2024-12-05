using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryCommand.DeleteLibrary
{
    public class DeleteLibraryCommandHandler : IRequestHandler<DeleteLibraryCommand, OperationResult<bool>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public DeleteLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var library = await _libraryRepository.GetByIdAsync(request.LibraryId);

                if (library == null)
                {
                    return OperationResult<bool>.Failure($"Library with ID {request.LibraryId} not found.");
                }

                await _libraryRepository.DeleteAsync(request.LibraryId);
                return OperationResult<bool>.Success(true, "Library successfully deleted.");
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.Failure($"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
