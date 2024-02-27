using DataSample.Application.Interfaces.Contexts;
using DataSample.Application.Services.Fainances.Queries.GetChegueById;
using DataSample.Domain.Entities.Fainances;
using DataSample.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Application.Services.Fainances.Queries.GetAllCheque
{
    public class GetAllChequeQuery :Irequest, IRequest<List<Cheque>>
    {
    }

    public class GetAllChequeQueryandler : IRequestHandler<GetAllChequeQuery, List<Cheque>>
    {
        private readonly IDataBaseContext _context;

        public GetAllChequeQueryandler(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<List<Cheque>> Handle(GetAllChequeQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cheques
                .Where(x => x.UserId == request.UserId)
                .OrderBy(x => x.Id)
                .ToListAsync();
        }
    }

}
