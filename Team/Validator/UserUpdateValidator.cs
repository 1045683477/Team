using FluentValidation;
using Team.Model.AutoMappers;

namespace Team.Validator
{
    /// <summary>
    /// 用户更新限制
    /// </summary>
    public class UserUpdateValidator:AbstractValidator<UserUpdateMap>
    {
        public UserUpdateValidator()
        {
            //用户名
            RuleFor(x => x.Name)
                .NotEmpty().WithName("用户名").WithMessage("{PropertyName}不能为空")
                .MinimumLength(1).WithMessage("{PropertyName}长度低于最小长度{MinLength}")
                .MaximumLength(6).WithMessage("{PropertyName}高于最大长度{MaxLength}");

            //密码
            RuleFor(x => x.Password)
                .NotEmpty().WithName("密码").WithMessage("{PropertyName}不能为空")
                .MinimumLength(6).WithMessage("{PropertyName}长度低于最小长度{MinLength}")
                .MaximumLength(12).WithMessage("{PropertyName}高于最大长度{MaxLength}");

            //性别
            RuleFor(x => x.Sex)
                .NotEmpty().WithName("性别").WithMessage("{PropertyName}不能为空").IsInEnum();

            //大学编号
            RuleFor(x => x.UniversityId)
                .NotEmpty().WithName("大学编号").WithMessage("{PropertyName}不可为空");
        }
    }
}
