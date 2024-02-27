using DataSample.Application.Interfaces.Contexts;
using DataSample.Domain.Entities.Fainances;
using DataSample.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Application.Services.Fainances.Queries.GetChegueById
{
    public class GetChequeByIdQuery :Irequest, IRequest<Cheque>
    {
        public int Id { get; set; }
    }

    public class GetChequeByIdQueryHandler : IRequestHandler<GetChequeByIdQuery, Cheque>
    {
        private readonly IDataBaseContext _context;

        public GetChequeByIdQueryHandler(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<Cheque> Handle(GetChequeByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cheques
                .Where(x => x.Id == request.Id && x.UserId == request.UserId)
                .OrderBy(x => x.Id)
                .FirstOrDefaultAsync();
        }
    }

}
