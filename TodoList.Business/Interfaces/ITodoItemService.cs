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
        Task<PagedResponse<TodoItemDto>> GetAllAsync(TodoQueryParameters queryParameters);
        Task<TodoItemDto> GetByIdAsync(Guid id);
        Task<TodoItemDto> CreateAsync(CreateTodoItemDto todoItem);
        Task UpdateAsync(Guid id, UpdateTodoItemDto todoItem);
        Task DeleteAsync(Guid id);
    }
}
