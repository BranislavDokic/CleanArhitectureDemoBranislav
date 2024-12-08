using Application.Books.Commands.DeleteBook;
using Application.Interfaces.Repositoryinterfaces;
using Domain;
using FakeItEasy;
using Microsoft.Extensions.Logging;


namespace TestProject
{
    public class DeleteBookCommandHandlerTests
    {
        private IGenericRepositoryInterface<Book> _fakeBookRepository;
        private ILogger<DeleteBookCommandHandler> _fakeLogger;
        private DeleteBookCommandHandler _handler;

        [SetUp]
        public void Setup()
        {
            _fakeBookRepository = A.Fake<IGenericRepositoryInterface<Book>>();
            _fakeLogger = A.Fake<ILogger<DeleteBookCommandHandler>>();

            _handler = new DeleteBookCommandHandler(_fakeBookRepository, _fakeLogger);
        }

        [Test]
        public async Task Handle_ShouldReturnSuccess_WhenBookIsDeletedSuccessfully()
        {
            var bookId = 1;
            var existingBook = new Book { Id = bookId, Title = "Existing Book" };

            A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, null))
                .Returns(Task.FromResult(existingBook)); 

            A.CallTo(() => _fakeBookRepository.DeleteAsync(bookId))
                .Returns(Task.FromResult("Deleted"));

            var command = new DeleteBookCommand(bookId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual("Book successfully deleted.", result.Message);
            A.CallTo(() => _fakeBookRepository.DeleteAsync(bookId)).MustHaveHappenedOnceExactly();
        }

        [Test]
        public async Task Handle_ShouldReturnFailure_WhenBookDoesNotExist()
        {
            var bookId = 1;

            A.CallTo(() => _fakeBookRepository.GetByIdAsync(bookId, null))
                .Returns(Task.FromResult<Book>(null)); 

            var command = new DeleteBookCommand(bookId);

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual($"Book with ID {bookId} not found.", result.ErrorMessage);
            A.CallTo(() => _fakeBookRepository.DeleteAsync(bookId)).MustNotHaveHappened();
        }
    }
}