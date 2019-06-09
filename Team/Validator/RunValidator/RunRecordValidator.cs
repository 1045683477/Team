using FluentValidation;
using Team.Model.AutoMappers.TeamMapper;

namespace Team.Validator.RunValidator
{
    public class RunRecordValidator:AbstractValidator<RunRecordMap>
    {
        public RunRecordValidator()
        {
            RuleFor(x => x.SportFreeModel)
                .NotEmpty().WithName("{运动项目}").WithMessage("{PropertyName}不可为空");

            RuleFor(x => x.Distance)
                .NotEmpty().WithName("{距离}").WithMessage("{PropertyName}不可为空");

            RuleFor(x => x.StarPlace)
                .NotEmpty().WithName("{起始地点}").WithMessage("{PropertyName}不可为空");

            RuleFor(x => x.EndPlace)
                .NotEmpty().WithName("{结束地点}").WithMessage("{PropertyName}不可为空");
            
            RuleFor(x => x.StarTime)
                .NotEmpty().WithName("{起始时间}").WithMessage("{PropertyName}不可为空");

            RuleFor(x => x.EndTime)
                .NotEmpty().WithName("{结束时间}").WithMessage("{PropertyName}不可为空");

            RuleFor(x => x.Calories)
                .NotEmpty().WithName("{消耗能量}").WithMessage("{PropertyName}不可为空");

            RuleFor(x => x.Speed)
                .NotEmpty().WithName("{速度}").WithMessage("{PropertyName}不可为空");
        }
    }
}
