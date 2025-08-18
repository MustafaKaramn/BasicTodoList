using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs.TodoItemDTOs;
using TodoList.Business.DTOs.TodoListDTOs;
using TodoList.Core.Entities;

namespace TodoList.Business.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<TodoItem, TodoItemDto>().ReverseMap();
            CreateMap<CreateTodoItemDto, TodoItem>();
            CreateMap<UpdateTodoItemDto, TodoItem>();

            CreateMap<TodoList.Core.Entities.TodoList, TodoListDto>().ReverseMap();
            CreateMap<CreateTodoListDto, TodoList.Core.Entities.TodoList>();
            CreateMap<UpdateTodoListDto, TodoList.Core.Entities.TodoList>();
        }
    }
}
