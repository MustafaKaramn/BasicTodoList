using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs.TodoItemDTOs;
using TodoList.Core.Entities;
using TodoList.Core.Helpers;

namespace TodoList.Business.Interfaces
{
    public interface ITodoItemService
    {
        Task<PagedResponse<TodoItemDto>> GetAllAsync(TodoQueryParameters queryParameters, Guid userId);
        Task<TodoItemDto> GetByIdAsync(Guid id, Guid userId);
        Task<TodoItemDto> CreateAsync(CreateTodoItemDto todoItem, Guid userId);
        Task UpdateAsync(Guid id, UpdateTodoItemDto todoItem, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
