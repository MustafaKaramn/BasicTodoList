using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.Abstract;
using TodoList.Business.DTOs;
using TodoList.Core.Entities;
using TodoList.DataAccess.Context;

namespace TodoList.Business.Concrete
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TodoRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TodoItemDto> CreateTodoAsync(CreateTodoDto createTodo)
        {
            var todoItem = _mapper.Map<TodoItem>(createTodo);
            todoItem.CreatedDate = DateTime.Now;
            todoItem.IsCompleted = false;

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task DeleteTodoAsync(Guid id)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItemDto>> GetAllTodoAsync()
        {
            var todos = await _context.TodoItems.ToListAsync();
            return _mapper.Map<List<TodoItemDto>>(todos);
        }

        public async Task<TodoItemDto> GetTodoByIdAsync(Guid id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            return _mapper.Map<TodoItemDto>(todoItem);
        }

        public async Task<List<TodoItemDto>> GetTodoByStatusAsync(bool isCompleted)
        {
            var todos = await _context.TodoItems
                .Where(t => t.IsCompleted == isCompleted)
                .ToListAsync();

            return _mapper.Map<List<TodoItemDto>>(todos);
        }

        public async Task UpdateTodoAsync(Guid id, UpdateTodoDto todoItem)
        {
            var todo = _context.TodoItems.Find(id);

            if (todo != null)
            {
                _mapper.Map(todoItem, todo);
                todo.UpdatedDate = DateTime.Now;

                await _context.SaveChangesAsync();
            }
        }
    }
}
