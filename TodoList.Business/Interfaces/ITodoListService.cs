using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.DTOs.TodoListDTOs;

namespace TodoList.Business.Interfaces
{
    public interface ITodoListService
    {
        Task<IEnumerable<TodoListDto>> GetAllAsync(Guid userId);
        Task<TodoListDto> GetByIdAsync(Guid id, Guid userId);
        Task<TodoListDto> CreateAsync(CreateTodoListDto todoList, string? imageUrl, Guid userId);
        Task UpdateAsync(Guid id, UpdateTodoListDto todoList, Guid userId);
        Task DeleteAsync(Guid id, Guid userId);
    }
}
