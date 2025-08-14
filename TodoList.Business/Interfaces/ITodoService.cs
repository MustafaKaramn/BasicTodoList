using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs;
using TodoList.Core.Entities;

namespace TodoList.Business.Abstract
{
    public interface ITodoService
    {
        Task<IEnumerable<TodoItemDto>> GetAllTodoAsync();
        Task<TodoItemDto> GetTodoByIdAsync(Guid id);
        Task<TodoItemDto> CreateTodoAsync(CreateTodoDto todoItem);
        Task UpdateTodoAsync(Guid id, UpdateTodoDto todoItem);
        Task DeleteTodoAsync(Guid id);
        Task<IEnumerable<TodoItemDto>> GetTodoByStatusAsync(bool isCompleted);
    }
}
