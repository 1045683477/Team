using FluentValidation;
using Team.Model.AutoMappers.UserMapper;

namespace Team.Validator.UserValidator
{
    /// <summary>
    /// 注册模板限制
    /// </summary>
    public class UserRegisteredValidator:AbstractValidator<UserRegisteredMap>
    {
        public UserRegisteredValidator()
        {
            //用户名
            RuleFor(x => x.Name)
                .NotEmpty().WithName("用户名").WithMessage("{PropertyName}不能为空")
                .MinimumLength(1).WithMessage("{PropertyName}长度低于最小长度{MinLength}")
                .MaximumLength(6).WithMessage("{PropertyName}高于最大长度{MaxLength}");

            //账号
            RuleFor(x => x.Account)
                .NotEmpty().WithName("账号").WithMessage("{PropertyName}不能为空")
                .MinimumLength(6).WithMessage("{PropertyName}长度低于最小长度{MinLength}")
                .MaximumLength(12).WithMessage("{PropertyName}高于最大长度{MaxLength}");

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

            //学号
            RuleFor(x => x.studentId)
                .NotEmpty().WithName("学号").WithMessage("{PropertyName}不可为空");
        }
    }
}
