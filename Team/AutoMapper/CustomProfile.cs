using AutoMapper;
using Team.Model.AutoMappers;
using Team.Model.Model;

namespace Team.AutoMapper
{
    public class CustomProfile:Profile
    {
        public CustomProfile()
        {
            CreateMap<UserRegisteredMap, User>();//注册

            CreateMap<User, UserUpdateMap>();//为修改用户数据提供映射

            CreateMap<Model.Model.Team, TeamSearchMap>();//队伍查询模板

            CreateMap<TeamCreateMap, Model.Model.Team>();//创建队伍用

            CreateMap<Participants, ParticipantsSearchMap>();//查取参与者信息

            CreateMap<Participants, ParticipantsSearchByJoinMap>();//查取用户参与的哩视队伍

            CreateMap<ParticipantsSearchByJoinMap, Participants>();

            CreateMap<User, Participants>(); //参加队伍

            CreateMap<Run, RunRecordMap>();//跑步记录

            CreateMap<RunRecordMap, Run>();//跑步记录

            CreateMap<Statistical, StatisticalMap>();//跑步统计

        }
    }
}
