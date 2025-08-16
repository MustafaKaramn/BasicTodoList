using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs;
using TodoList.Core.Entities;
using TodoList.Core.Helpers;

namespace TodoList.Business.Interfaces
{
    public interface ITodoService
    {
        Task<PagedResponse<TodoItemDto>> GetAllTodoAsync(PaginationParameters paginationParameters);
        Task<TodoItemDto> GetTodoByIdAsync(Guid id);
        Task<TodoItemDto> CreateTodoAsync(CreateTodoDto todoItem);
        Task UpdateTodoAsync(Guid id, UpdateTodoDto todoItem);
        Task DeleteTodoAsync(Guid id);
        Task<PagedResponse<TodoItemDto>> GetTodoByStatusAsync(bool isCompleted, PaginationParameters paginationParameters);
    }
}
