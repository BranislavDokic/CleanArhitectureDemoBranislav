using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetLibraryById
{
    public class GetLibraryByIdQueryHandler : IRequestHandler<GetLibraryByIdQuery, OperationResult<LibraryDTO>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public GetLibraryByIdQueryHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<OperationResult<LibraryDTO>> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var library = await _libraryRepository.GetByIdAsync(
                    request.LibraryId,
                    query => query.Include(l => l.Books));

                if (library == null)
                {
                    return OperationResult<LibraryDTO>.Failure($"Library with ID {request.LibraryId} not found.");
                }

                var libraryDTO = new LibraryDTO(library)
                {
                    BookNames = library.Books.Select(b => b.Title).ToList() 
                };

                return OperationResult<LibraryDTO>.Success(libraryDTO, "Library retrieved successfully.");
            }
            catch (Exception ex)
            {
                return OperationResult<LibraryDTO>.Failure($"Error: {ex.Message}");
            }
        }
    }
}
