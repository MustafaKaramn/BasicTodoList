using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Core.Entities;
using TodoList.DataAccess.Context;
using TodoList.DataAccess.Interfaces;

namespace TodoList.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IRepository<TodoItem> TodoRepository { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            TodoRepository = new Repository<TodoItem>(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
