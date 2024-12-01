using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryCommand.DeleteLibrary
{
    public class DeleteLibraryCommandHandler : IRequestHandler<DeleteLibraryCommand, string>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public DeleteLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<string> Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var library = await _libraryRepository.GetByIdAsync(request.LibraryId);

                if (library == null)
                {
                   
                    return "Library not found"; 
                }

                await _libraryRepository.DeleteAsync(request.LibraryId);
                return "Deleted";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
