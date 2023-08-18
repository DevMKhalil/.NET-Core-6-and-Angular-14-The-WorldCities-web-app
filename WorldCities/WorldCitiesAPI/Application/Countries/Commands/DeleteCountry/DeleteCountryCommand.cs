using CSharpFunctionalExtensions;
using MediatR;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Resources;

namespace WorldCitiesAPI.Application.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommand : IRequest<Result>
    {
        public int CountryId { get; set; }
    }

    public class DeleteCountryCommandHandler : IRequestHandler<DeleteCountryCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteCountryCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FindAsync(request.CountryId);
            if (country is null)
                return Result.Failure(Resource.CountryNotFound);

            _context.Countries.Remove(country);

            var result = await _context.SaveChangesAsyncWithValidation();

            if (result.IsFailure)
                return Result.Failure(result.Error);

            return Result.Success();
        }
    }
}
