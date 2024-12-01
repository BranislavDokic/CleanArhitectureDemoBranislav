using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryQuery.GetAllLibrary
{
    public class GetAllLibraryQueryHandler : IRequestHandler<GetAllLibraryQuery, List<LibraryDTO>>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public GetAllLibraryQueryHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<List<LibraryDTO>> Handle(GetAllLibraryQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var libraries = await _libraryRepository.GetAllAsync();

                if (libraries == null || !libraries.Any())
                {
                    return new List<LibraryDTO>();
                }

                var libraryDTOs = libraries.Select(library => new LibraryDTO(library)).ToList();

                return libraryDTOs;
            }
            catch (Exception)
            {
                throw new Exception("Error fetching libraries");
            }
        }
    }
}
