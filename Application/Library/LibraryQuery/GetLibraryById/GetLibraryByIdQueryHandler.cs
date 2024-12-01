using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetLibraryById
{
    public class GetLibraryByIdQueryHandler : IRequestHandler<GetLibraryByIdQuery, LibraryDTO>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public GetLibraryByIdQueryHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<LibraryDTO> Handle(GetLibraryByIdQuery request, CancellationToken cancellationToken)
        {
            var library = await _libraryRepository.GetByIdAsync(request.LibraryId, query => query.Include(l => l.Books));

            if (library == null)
                throw new KeyNotFoundException($"Library with ID {request.LibraryId} not found.");

            return new LibraryDTO(library);
        }
    }
}
