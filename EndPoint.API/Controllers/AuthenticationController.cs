using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DataSample.Application.Services.Users.Commands.RgegisterUser;
using DataSample.Application.Services.Users.Commands.UserLogin;
using DataSample.Common.Dto;
using EndPoint.ApI.Models.ViewModels.AuthenticationViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace EndPoint.Site.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {

        private readonly IRegisterUserService _registerUserService;
        private readonly IUserLoginService _userLoginService;

        public AuthenticationController(IRegisterUserService registerUserService, IUserLoginService userLoginService)
        {
            _registerUserService = registerUserService;
            _userLoginService = userLoginService;
        }

        [HttpPost]
        public ResultDto Signup(SignupViewModel request)
        {
            if (string.IsNullOrWhiteSpace(request.FullName) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password) ||
                string.IsNullOrWhiteSpace(request.RePassword))
            {
                return new ResultDto { IsSuccess = false, Message = "لطفا تمامی موارد رو ارسال نمایید" };
            }

            if (User.Identity.IsAuthenticated == true)
            {
                return new ResultDto { IsSuccess = false, Message = "شما به حساب کاربری خود وارد شده اید! و در حال حاضر نمیتوانید ثبت نام مجدد نمایید" };
            }
            if (request.Password != request.RePassword)
            {
                return new ResultDto { IsSuccess = false, Message = "رمز عبور و تکرار آن برابر نیست" };
            }
            if (request.Password.Length < 8)
            {
                return new ResultDto { IsSuccess = false, Message = "رمز عبور باید حداقل 8 کاراکتر باشد" };
            }

            string emailRegex = @"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";

            var match = Regex.Match(request.Email, emailRegex, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                return new ResultDto { IsSuccess = true, Message = "ایمیل خودرا به درستی وارد نمایید" };
            }


            var signeupResult = _registerUserService.Execute(new RequestRegisterUserDto
            {
                Email = request.Email,
                FullName = request.FullName,
                Password = request.Password,
                RePasword = request.RePassword,
                roles = new List<RolesInRegisterUserDto>()
                {
                     new RolesInRegisterUserDto { Id = 3},
                }
            });

            if (signeupResult.IsSuccess == true)
            {
                var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,signeupResult.Data.UserId.ToString()),
                new Claim(ClaimTypes.Email, request.Email),
                new Claim(ClaimTypes.Name, request.FullName),
                new Claim(ClaimTypes.Role, "Customer"),
            };


                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var properties = new AuthenticationProperties()
                {
                    IsPersistent = true
                };
                HttpContext.SignInAsync(principal, properties);

            }
            return new ResultDto { IsSuccess = signeupResult.IsSuccess, Message = signeupResult.Message };
        }

        //[HttpPost]
        //public ResultDto Signin(SignInViewModel request)
        //{
        //    var signupResult = _userLoginService.Execute(request.Email, request.Password);
        //    if (signupResult.IsSuccess == true)
        //    {
        //        var claims = new List<Claim>()
        //    {
        //        new Claim(ClaimTypes.NameIdentifier,signupResult.Data.UserId.ToString()),
        //        new Claim(ClaimTypes.Email, request.Email),
        //        new Claim(ClaimTypes.Name, signupResult.Data.Name),

        //    };
        //        foreach (var item in signupResult.Data.Roles)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, item));
        //        }

        //        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        //        var principal = new ClaimsPrincipal(identity);
        //        var properties = new AuthenticationProperties()
        //        {
        //            IsPersistent = true,
        //            ExpiresUtc = DateTime.Now.AddDays(5),
        //        };
        //        HttpContext.SignInAsync(principal, properties);

        //    }
        //    return new ResultDto { IsSuccess = signupResult.IsSuccess, Message = signupResult.Message };
        //}

    }
}
