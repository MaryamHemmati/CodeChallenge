using DataSample.Application.Interfaces.Contexts;
using DataSample.Application.Services.Fainances.Queries.GetAllCheque;
using DataSample.Domain.Entities.Fainances;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Application.Services.Fainances.Commands
{
    //[Validator(typeof(CreateTodoItemCommandValidator))]

    public class CreateChequeCommand : Irequest, IRequest<Cheque>
    {
        [Required]
        public string Name { get; set; }
        public DateTime DueDate { get; set; }

        [Required]
        public string SayadNo { get; set; }

        [Required]
        public string ShebaNoCreditor { get; set; }

        [Required]
        public string ShebaNoDebtor { get; set; }
    }


    public class CreateChequeCommandHandler : IRequestHandler<CreateChequeCommand, Cheque>
    {
        private readonly IDataBaseContext _context;

        public CreateChequeCommandHandler(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<Cheque> Handle(CreateChequeCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                return null!;

            var entity = new Cheque
            {
                DueDate = request.DueDate,
                SayadNo = request.SayadNo,
                InsertTime = DateTime.Now,
                Name = request.Name,
                ShebaNoCreditor = request.ShebaNoCreditor,
                ShebaNoDebtor = request.ShebaNoDebtor,
                UserId = request.UserId,

            };
            await _context.Cheques.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;

        }
    }

    public class CreateChequeCommandValidator : AbstractValidator<CreateChequeCommand>
    {
        public CreateChequeCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty();

            RuleFor(v => v.SayadNo)
            .MaximumLength(16)
            .MinimumLength(16)
            .NotEmpty()
            .WithMessage("Invalid SayadNo Length");

            RuleFor(v => v.ShebaNoCreditor)
            .MaximumLength(26)
            .MinimumLength(26)
            .NotEmpty()
            .WithMessage("Invalid ShebaNoCreditor Length");

            RuleFor(v => v.ShebaNoDebtor)
           .MaximumLength(26)
           .MinimumLength(26)
           .NotEmpty()
           .WithMessage("Invalid ShebaNoDebtor Length");
        }
    }

}
