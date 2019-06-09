using AutoMapper;
using Team.Model.AutoMappers;
using Team.Model.AutoMappers.RunTeamMapper;
using Team.Model.AutoMappers.TeamMapper;
using Team.Model.AutoMappers.UserMapper;
using Team.Model.Model;
using Team.Model.Model.RunTeamModel;
using Team.Model.Model.TeamModel;
using Team.Model.Model.UserModel;

namespace Team.AutoMapper
{
    public class CustomProfile:Profile
    {
        public CustomProfile()
        {
            CreateMap<UserRegisteredMap, User>();//注册

            CreateMap<User, UserUpdateMap>();//为修改用户数据提供映射

            CreateMap<User, UserSearchMapper>();//查询用户模板

            CreateMap<Model.Model.TeamModel.Team, TeamSearchMap>();//队伍查询模板

            CreateMap<TeamCreateMap, Model.Model.TeamModel.Team>();//创建队伍用

            CreateMap<Participants, ParticipantsSearchMap>();//查取参与者信息

            CreateMap<Participants, ParticipantsSearchByJoinMap>();//查取用户参与的哩视队伍

            CreateMap<ParticipantsSearchByJoinMap, Participants>();

            CreateMap<User, Participants>(); //参加队伍

            CreateMap<Run, RunRecordMap>();//跑步记录

            CreateMap<RunRecordMap, Run>();//跑步记录

            CreateMap<Statistical, StatisticalMap>();//跑步统计

            CreateMap<RunTeamCreateMap, RunTeam>();//跑步队伍注册模板

            CreateMap<RunApplicant, RunParticipants>();//同意入队映射

            CreateMap<RunTeamUpdateMap, RunTeam>();//修改队伍信息

            CreateMap<User, RunTeamSuccessCreateUserMapper>();//创建跑步队伍成功返回模板

            CreateMap<RunTeam, RunTeamSuccessCreateMapper>();//创建跑步队伍成功模板

            CreateMap<RunTeam, RunTeamSearch>();//通过 Id 查询队伍模板

            CreateMap<RunParticipants, RunParticipantsMapper>();//参与者查询模板

            CreateMap<RunApplicant, RunApplicantMapper>();//申请者查询模板

            CreateMap<RunApplicant, RunParticipants>();//加入队伍

            CreateMap<LatitudeAndLongitude, LAndLSearchMapper>();
        } 
    }
}
