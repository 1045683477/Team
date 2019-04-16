using FluentValidation;
using Team.Model.AutoMappers;

namespace Team.Validator
{
    public class UserLoginValidator:AbstractValidator<UserLoginMap>
    {
        public UserLoginValidator()
        {
            RuleFor(x=>x.Account)
                .NotEmpty().WithName("账号").WithMessage("{PropertyName}不能为空")
                .MinimumLength(6).WithMessage("{PropertyName}长度低于最小长度{MinLength}")
                .MaximumLength(12).WithMessage("{PropertyName}高于最大长度{MaxLength}");

            RuleFor(x=>x.Password)
                .NotEmpty().WithName("密码").WithMessage("{PropertyName}不能为空")
                .MinimumLength(6).WithMessage("{PropertyName}长度低于最小长度{MinLength}")
                .MaximumLength(12).WithMessage("{PropertyName}高于最大长度{MaxLength}");
        }
    }
}
