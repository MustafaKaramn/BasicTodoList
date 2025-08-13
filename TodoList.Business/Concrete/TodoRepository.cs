using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Business.Abstract;
using TodoList.Core.Entities;
using TodoList.DataAccess.Context;

namespace TodoList.Business.Concrete
{
    public class TodoRepository : ITodoRepository
    {
        private readonly AppDbContext _context;

        public TodoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TodoItem> CreateTodoAsync(TodoItem todoItem)
        {
            todoItem.CreatedDate = DateTime.Now;
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }

        public async Task DeleteTodoAsync(int id)
        {
            var todo = await _context.TodoItems.FindAsync(id);

            if (todo != null)
            {
                _context.TodoItems.Remove(todo);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<TodoItem>> GetAllTodoAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetTodoByIdAsync(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            return todoItem;
        }

        public async Task<List<TodoItem>> GetTodoByStatusAsync(bool isCompleted)
        {
            return await _context.TodoItems
                .Where(t => t.IsCompleted == isCompleted)
                .ToListAsync();
        }

        public async Task UpdateTodoAsync(TodoItem todoItem)
        {
            var todo = _context.TodoItems.Find(todoItem.Id);

            if (todo != null)
            {
                todo.Description = todoItem.Description;
                todo.Title = todoItem.Title;
                todo.IsCompleted = todoItem.IsCompleted;
                todo.UpdatedDate = DateTime.Now;
                todo.CreatedDate = todo.CreatedDate;

                _context.Entry(todo).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }
    }
}
