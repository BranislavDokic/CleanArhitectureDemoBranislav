using Application.Interfaces.Repositoryinterfaces;
using Domain;
using Domain.Result;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;


namespace Application.Authors.AuthorQueris.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<List<Author>>>
    {
        private readonly IGenericRepositoryInterface<Author> _authorRepository;
        private readonly ILogger<GetAllAuthorsQueryHandler> _logger;
        private readonly IMemoryCache _memoryCache;
        private const string cacheKey = "allAuthors";

        public GetAllAuthorsQueryHandler(IGenericRepositoryInterface<Author> authorRepository, ILogger<GetAllAuthorsQueryHandler> logger, IMemoryCache memoryCache)
        {
            _authorRepository = authorRepository;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        public async Task<OperationResult<List<Author>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Attempting to retrieve all authors.");

            try
            {
                if (!_memoryCache.TryGetValue(cacheKey, out List<Author> authors))
                {
                    _logger.LogInformation("Cache miss. Fetching authors from the database.");

                    authors = await _authorRepository.GetAllAsync();

                    if (authors == null || !authors.Any())
                    {
                        _logger.LogWarning("No authors found.");
                        return OperationResult<List<Author>>.Failure("No authors found.");
                    }

                   
                    var cacheEntryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3), 
                        Priority = CacheItemPriority.Normal
                    };
                    _memoryCache.Set(cacheKey, authors, cacheEntryOptions);

                    _logger.LogInformation("Authors successfully cached.");
                }
                else
                {
                    _logger.LogInformation("Authors retrieved from cache.");
                }

                _logger.LogInformation("Successfully retrieved {AuthorCount} authors.", authors.Count);
                return OperationResult<List<Author>>.Success(authors, "Authors retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving authors.");
                return OperationResult<List<Author>>.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
