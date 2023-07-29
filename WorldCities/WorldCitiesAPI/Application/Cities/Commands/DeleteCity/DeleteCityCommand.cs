using CSharpFunctionalExtensions;
using MediatR;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Application.Cities.Commands.DeleteCity
{
    public class DeleteCityCommand : IRequest<Result>
    {
        public int CityId { get; set; }
    }

    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        public DeleteCityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Result> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            var city = await _context.Cities.FindAsync(request.CityId);
            if (city is null)
                return Result.Failure<City>("City Not Found");

            _context.Cities.Remove(city);

            var result = await _context.SaveChangesAsyncWithValidation();

            if (result.IsFailure)
                return Result.Failure(result.Error);

            return Result.Success();
        }
    }
}
