using DataSample.Application.Interfaces.Contexts;
using DataSample.Application.Services.Fainances.Commands;
using DataSample.Domain.Entities.Fainances;
using DataSample.Domain.Entities.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Application.Services.Fainances.Commands.RemoveCheque
{
    public class RemoveChequeCommand : Irequest, IRequest<bool>
    {
        public int Id { get; set; }
    }

    public class RemoveChequeCommandHandler : IRequestHandler<RemoveChequeCommand, bool>
    {
        private readonly IDataBaseContext _context;

        public RemoveChequeCommandHandler(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(RemoveChequeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _context.Cheques.FirstOrDefault(x=>x.Id==request.Id && x.UserId==request.UserId);
                if (entity != null)
                {

                    entity.IsRemoved = true;
                    entity.RemoveTime = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
