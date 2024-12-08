using Application.Dtos;
using Application.Interfaces.Repositoryinterfaces;
using Application.Library.LibraryCommand.CreateLibrary;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;

namespace TestProject;

public class TestingCreateLibray
{
    public class TestingCreateLibrary
    {
        private IGenericRepositoryInterface<LibraryModel> _fakeLibraryRepository;
        private CreateLibraryCommandHandler _handler;
        private ILogger<CreateLibraryCommandHandler> _fakeLogger;

        [SetUp]
        public void Setup()
        {
            _fakeLibraryRepository = A.Fake<IGenericRepositoryInterface<LibraryModel>>();  
            _fakeLogger = A.Fake<ILogger<CreateLibraryCommandHandler>>(); 
            _handler = new CreateLibraryCommandHandler(_fakeLibraryRepository, _fakeLogger); 
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenLibraryDoesNotExist()
        {
            var newLibraryDto = new LibraryDTO
            {
                Name = "New Library"
            };

            var query = new CreateLibraryCommand(newLibraryDto);
            var cancellationToken = new CancellationToken();

            A.CallTo(() => _fakeLibraryRepository.GetAllAsync()).Returns(Task.FromResult(new List<LibraryModel>()));

            var newLibrary = new LibraryModel { Name = newLibraryDto.Name };
            A.CallTo(() => _fakeLibraryRepository.AddAsync(A<LibraryModel>.Ignored)).Returns(Task.FromResult(newLibrary));

            var result = await _handler.Handle(query, cancellationToken);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("New Library", result.Data.Name);
            Assert.AreEqual("Library successfully added.", result.Message);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenLibraryAlreadyExists()
        {
            var newLibraryDto = new LibraryDTO
            {
                Name = "Existing Library"
            };

            var query = new CreateLibraryCommand(newLibraryDto);
            var cancellationToken = new CancellationToken();

            var existingLibrary = new LibraryModel { Name = "Existing Library" };
            A.CallTo(() => _fakeLibraryRepository.GetAllAsync()).Returns(Task.FromResult(new List<LibraryModel> { existingLibrary }));

            var result = await _handler.Handle(query, cancellationToken);

            
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Library with the name '{newLibraryDto.Name}' already exists.", result.ErrorMessage);
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenErrorOccurs()
        {
            var exceptionMessage = "Error: Database connection failed";

            var newLibraryDto = new LibraryDTO
            {
                Name = "New Library"
            };

            var query = new CreateLibraryCommand(newLibraryDto);
            var cancellationToken = new CancellationToken();

            A.CallTo(() => _fakeLibraryRepository.GetAllAsync()).Throws(new Exception(exceptionMessage));

            var result = await _handler.Handle(query, cancellationToken);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Error: {exceptionMessage}", result.ErrorMessage);
        }
}   }
