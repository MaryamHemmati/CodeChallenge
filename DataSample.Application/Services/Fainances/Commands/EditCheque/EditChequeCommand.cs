using DataSample.Application.Interfaces.Contexts;
using DataSample.Application.Services.Fainances.Commands;
using DataSample.Domain.Entities.Fainances;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataSample.Application.Services.Fainances.Commands
{
    public class ValidatorDto
    {

    }
    public class EditChequeCommand : Irequest, IRequest<Cheque>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DueDate { get; set; }

        [StringLength(16, ErrorMessage = "The SayadNo value cannot exceed 16 characters. ")]
        public string SayadNo { get; set; }

        [StringLength(26, ErrorMessage = "The ShebaNoCreditor value cannot exceed 16 characters. ")]
        public string ShebaNoCreditor { get; set; }

        [StringLength(26, ErrorMessage = "The ShebaNoDebtor value cannot exceed 16 characters. ")]
        public string ShebaNoDebtor { get; set; }
    }


    public class EditChequeCommandHandler : IRequestHandler<EditChequeCommand, Cheque>
    {
        private readonly IDataBaseContext _context;

        public EditChequeCommandHandler(IDataBaseContext context)
        {
            _context = context;
        }

        public async Task<Cheque> Handle(EditChequeCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.Cheques.FirstOrDefault(x => x.Id == request.Id && x.UserId == request.UserId);
            if (entity == null || entity.Id == 0)
                return null!;

            entity.UpdateTime = DateTime.Now;
            entity.Name = request.Name;
            entity.DueDate = request.DueDate;
            entity.SayadNo = request.SayadNo;
            entity.ShebaNoCreditor = request.ShebaNoCreditor;

            _context.Cheques.Attach(entity);
            _context.Cheques.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _context.Cheques.Entry(entity).State = EntityState.Detached;
            return entity;

        }
    }


    public class EditChequeCommandValidator : AbstractValidator<EditChequeCommand>
    {
        public EditChequeCommandValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty();

            RuleFor(v => v.SayadNo)
            .MaximumLength(16)
            .MinimumLength(16)
            .NotEmpty()
            .WithMessage("Invalid SayadNo Length");

            RuleFor(v => v.ShebaNoCreditor)
            .MaximumLength(24)
            .MinimumLength(24)
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
