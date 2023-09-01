using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using WorldCitiesAPI.Common;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Countries.Queries.IsDuplicatedField
{
    public class IsDuplicatedFieldQuery : IRequest<bool>
    {
        public int CountryId { get; set; }
        public string FieldName { get; set; } = null!;
        public string FieldValue { get; set; }  = null!;
    }

    public class IsDuplicatedFieldQueryHandler : IRequestHandler<IsDuplicatedFieldQuery, bool>
    {
        private readonly IApplicationDbContext _context;
        public IsDuplicatedFieldQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(IsDuplicatedFieldQuery request, CancellationToken cancellationToken)
        {
            //var query = _context.Countries.AsQueryable();

            //query = query.Where(x => x.Id != request.CountryId);

            //switch (request.FieldName)
            //{
            //    case "name":
            //        return await query.AnyAsync(x => x.Name == request.FieldValue);
            //    case "iso2":
            //        return await query.AnyAsync(x => x.ISO2 == request.FieldValue);
            //    case "iso3":
            //        return await query.AnyAsync(x => x.ISO3 == request.FieldValue);
            //    default:
            //        return false;
            //}

            return (ApiResult<Country>.IsValidProperty(request.FieldName, true))
                ? _context.Countries.Any($"{request.FieldName} == @0 && Id != @1",request.FieldValue,request.CountryId)
                : false;
        }
    }
}
