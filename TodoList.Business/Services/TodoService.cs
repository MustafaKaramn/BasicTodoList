using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.Abstract;
using TodoList.Business.DTOs;
using TodoList.Core.Entities;
using TodoList.DataAccess.Context;
using TodoList.DataAccess.Interfaces;

namespace TodoList.Business.Concrete
{
    public class TodoService : ITodoService
    {
        private readonly IRepository<TodoItem> _todoRepository;
        private readonly IMapper _mapper;

        public TodoService(IRepository<TodoItem> todoRepository, IMapper mapper)
        {
            _todoRepository = todoRepository;
            _mapper = mapper;
        }

        public async Task<TodoItemDto> CreateTodoAsync(CreateTodoDto createTodo)
        {
            var todoItem = _mapper.Map<TodoItem>(createTodo);
            todoItem.CreatedDate = DateTime.Now;
            todoItem.IsCompleted = false;

            await _todoRepository.AddAsync(todoItem);

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo != null)
            {
                await _todoRepository.DeleteAsync(todo);
            }
        }

        public async Task<IEnumerable<TodoItemDto>> GetAllTodoAsync()
        {
            var todos = await _todoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<TodoItemDto>>(todos);
        }

        public async Task<TodoItemDto> GetTodoByIdAsync(Guid id)
        {
            var todoItem = await _todoRepository.GetByIdAsync(id);
            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<IEnumerable<TodoItemDto>> GetTodoByStatusAsync(bool isCompleted)
        {
            Expression<Func<TodoItem, bool>> filter = t => t.IsCompleted == isCompleted;

            var todos = await _todoRepository.GetAllAsync(filter);
            return _mapper.Map<IEnumerable<TodoItemDto>>(todos);
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodoDto todoItem)
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if (todo != null)
            {
                _mapper.Map(todoItem, todo);
                todo.UpdatedDate = DateTime.Now;

                await _todoRepository.UpdateAsync(todo);
            }
        }
    }
}
