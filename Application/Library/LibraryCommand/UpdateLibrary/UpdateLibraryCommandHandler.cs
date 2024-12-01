using Application.Interfaces.Repositoryinterfaces;
using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Library.LibraryCommand.UpdateLibrary
{
    public class UpdateLibraryCommandHandler : IRequestHandler<UpdateLibraryCommand, string>
    {
        private readonly IGenericRepositoryInterface<LibraryModel> _libraryRepository;

        public UpdateLibraryCommandHandler(IGenericRepositoryInterface<LibraryModel> libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task<string> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var library = await _libraryRepository.GetByIdAsync(request.LibraryId);

                if (library == null)
                {
                    return "Library not found";
                }

                library.Name = request.NewName;

                await _libraryRepository.UpdateAsync(request.LibraryId, library);

                return "Library updated successfully";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
