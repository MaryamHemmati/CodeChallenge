using Azure.Core;
using DataSample.Application.Services.Fainances.Commands;
using DataSample.Application.Services.Fainances.Commands.RemoveCheque;
using DataSample.Application.Services.Fainances.Queries.GetAllCheque;
using DataSample.Application.Services.Fainances.Queries.GetChegueById;
using DataSample.Common.Dto;
using DataSample.Domain.Entities.Fainances;
using DataSample.Domain.Entities.Users;
using EndPoint.API.Helper;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EndPoint.API.Controllers
{
    [ApiController]
    [Route("Cheque/[controller]")]
    public class ChequeController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUser _user;

        public ChequeController(IMediator mediator, IUser user)
        {
            _mediator = mediator;
            _user = user;

        }

        [Route("GetCheque")]
        [HttpGet]
        public async Task<ResultDto<Cheque>> Get(int Id)
        {
            var currenuUser = _user.Id;
            if (string.IsNullOrWhiteSpace(currenuUser))
                return new ResultDto<Cheque> { IsSuccess = false, Message = "خطای دسترسی", };

            var request = new GetChequeByIdQuery
            {
                Id = Id,
                UserId = int.Parse(currenuUser)
            };
            var res =await _mediator.Send(request);

            if (res != null)
            {
                return new ResultDto<Cheque> { IsSuccess = true, Message = "", Data = res };
            }
              
            else
                return new ResultDto<Cheque> { IsSuccess = false, Message = "عملیات با شکست مواجه شد." ,};

        }

        [Route("GetAllCheque")]
        [HttpGet]
        public async Task<ResultDto<List<Cheque>>> GetAll()
        {
            var currenuUser = _user.Id;
            if (string.IsNullOrWhiteSpace(currenuUser))
                return new ResultDto<List<Cheque>> { IsSuccess = false, Message = "خطای دسترسی" };

            var request = new GetAllChequeQuery
            {
                UserId = int.Parse(currenuUser)
            };
            var res = await _mediator.Send(request);

            return new ResultDto<List<Cheque>> { IsSuccess = true, Data = res };

        }


        [Route("CreateCheaue")]
        [HttpPost]
        public async Task<ResultDto> Post(CreateChequeCommand request)
        {
            var currenuUser = _user.Id;
            if (string.IsNullOrWhiteSpace(currenuUser))
                return new ResultDto { IsSuccess = false, Message = "خطای دسترسی" };

            request.UserId = int.Parse(currenuUser);
            var res = await _mediator.Send(request);

            if (res != null && res.Id > 0)
                return new ResultDto { IsSuccess = true, Message = "عملیات با موفقیت انجام شد." };
            else
                return new ResultDto { IsSuccess = false, Message = "عملیات با شکست مواجه شد." };

        }


        [Route("EditCheaue")]
        [HttpPost]
        public async Task<ResultDto> Put(EditChequeCommand request)
        {
            var currenuUser = _user.Id;
            if (string.IsNullOrWhiteSpace(currenuUser))
                return new ResultDto { IsSuccess = false, Message = "خطای دسترسی" };

            request.UserId = int.Parse(currenuUser);
            var res = await _mediator.Send(request);

            if (res != null && res.Id > 0)
                return new ResultDto { IsSuccess = true, Message = "عملیات با موفقیت انجام شد." };
            else
                return new ResultDto { IsSuccess = false, Message = "عملیات با شکست مواجه شد." };

        }

        [Route("RemoveCheaue")]
        [HttpPost]
        public async Task<ResultDto> Delete(int id)
        {
            var currenuUser = _user.Id;
            if (string.IsNullOrWhiteSpace(currenuUser))
                return new ResultDto { IsSuccess = false, Message = "خطای دسترسی" };

            var request = new RemoveChequeCommand
            {
                Id = id,
                UserId = int.Parse(currenuUser)
            };
            var res = await _mediator.Send(request);

            if (res == true)
                return new ResultDto { IsSuccess = true, Message = "عملیات با موفقیت انجام شد." };
            else
                return new ResultDto { IsSuccess = false, Message = "عملیات با شکست مواجه شد." };

        }

    }
}
