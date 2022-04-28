using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Models;

namespace TaskManagementSystem
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account,AccountDto>();
            CreateMap<AccountDto, Account>().ForMember(x=>x.account_id,
                                                       opt=>opt.Ignore());

            CreateMap<Board, BoardDto>();
            CreateMap<BoardDto, Board>().ForMember(x => x.board_id,
                                                       opt => opt.Ignore());

            CreateMap<TaskManagementSystem.Models.Task, TaskDto>();
            CreateMap<TaskDto, TaskManagementSystem.Models.Task>()
                .ForMember(x => x.task_id, opt => opt.Ignore())
                .ForMember(x => x.assigned_to, opt => opt.Ignore())
                .ForMember(x => x.story_points, opt => opt.Ignore());

            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>().ForMember(x => x.project_id,
                                                       opt => opt.Ignore());

            CreateMap<BoardMember, BoardMemberDto>();
            CreateMap<BoardMemberDto, BoardMember>();

        }
    }
}
