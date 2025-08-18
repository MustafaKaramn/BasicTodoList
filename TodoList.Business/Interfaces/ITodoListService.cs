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
        Task<IEnumerable<TodoListDto>> GetAllAsync();
        Task<TodoListDto> GetByIdAsync(Guid id);
        Task<TodoListDto> CreateAsync(CreateTodoListDto todoList, string? imageUrl);
        Task UpdateAsync(Guid id, UpdateTodoListDto todoList);
        Task DeleteAsync(Guid id);
    }
}
