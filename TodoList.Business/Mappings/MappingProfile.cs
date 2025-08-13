using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs;
using TodoList.Core.Entities;

namespace TodoList.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoItemDto>();
            CreateMap<CreateTodoDto, TodoItem>();
            CreateMap<UpdateTodoDto, TodoItem>();
        }
    }
}
