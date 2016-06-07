using FluentValidation;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Model.Validation.Custom;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Nest;

namespace Microbrewit.Api.Model.Validation
{
    public class UserPostDtoValidation : AbstractValidator<UserPostDto>
    {
        private readonly IUserService _userService;
        public UserPostDtoValidation(IUserService userService)
        {
            _userService = userService;
            RuleFor(user => user.Email).NotEmpty().EmailAddress();
            RuleFor(user => user.Password).Length(6,int.MaxValue).Equal(user => user.ConfirmPassword).WithMessage("Passwords does not match");
            RuleFor(user => user.Username).NotEmpty().Must(UniqueUsername).WithMessage("Username already exists");
        }

        private bool UniqueUsername(string username)
        {
            var result =_userService.ExistsUsername(username);
            return !result;
        }
    }
}
