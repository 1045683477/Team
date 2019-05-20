using FluentValidation;
using Team.Model.AutoMappers.RunTeamMapper;

namespace Team.Validator.RunTeam
{
    public class RunTeamCreateValidator:AbstractValidator<RunTeamCreateMap>
    {
        public RunTeamCreateValidator()
        {
            //队伍名
            RuleFor(x => x.Name)
                .NotEmpty().WithName("队伍名").WithMessage("{PropertyName} 不可为空")
                .MinimumLength(1).WithMessage("{PropertyName} 最小长度 {MinLength}")
                .MaximumLength(10).WithMessage("{PropertyName} 最大长度不可超过 {MaxLength}");

            //队伍简介
            RuleFor(x=>x.Introduction)
                .MaximumLength(100).WithName("队伍简介").WithMessage("{PropertyName} 最大长度不可超过 {MaxLength}");

        }
    }
}
