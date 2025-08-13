using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Entities;

namespace TodoList.Business.Abstract
{
    public interface ITodoRepository
    {
        Task<List<TodoItem>> GetAllTodoAsync();
        Task<TodoItem> GetTodoByIdAsync(Guid id);
        Task<TodoItem> CreateTodoAsync(TodoItem todoItem);
        Task UpdateTodoAsync(TodoItem todoItem);
        Task DeleteTodoAsync(Guid id);
        Task<List<TodoItem>> GetTodoByStatusAsync(bool isCompleted);
    }
}
