using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetAllLibrary
{
    public class GetAllLibraryQueryHandler : IRequestHandler<GetAllLibraryQuery, OperationResult<List<LibraryDTO>>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public GetAllLibraryQueryHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<OperationResult<List<LibraryDTO>>> Handle(GetAllLibraryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var libraries = await _libraryRepository.GetAllAsync();

                if (libraries == null || !libraries.Any())
                {
                    return OperationResult<List<LibraryDTO>>.Failure("No libraries found.");
                }

                var libraryDTOs = libraries.Select(library => new LibraryDTO(library)).ToList();

                return OperationResult<List<LibraryDTO>>.Success(libraryDTOs, "Libraries retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<List<LibraryDTO>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
