using FluentValidation.Validators;
using Microbrewit.Api.Repository.Interface;

namespace Microbrewit.Api.Model.Validation.Custom
{
    public class UniqueUserName<T> : PropertyValidator
    {
        private readonly IUserRepository _userRepository;
        public UniqueUserName(IUserRepository userRepository) : base("")
        {
            _userRepository = userRepository;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var username = context.PropertyValue as string;
            return _userRepository.ExistsUsername(username);
        }
    }
}
